using ExpenseTracker.Models;
using System.Text.Json;

namespace ExpenseTracker.Services;

/// <summary>
/// Service for managing gamification features including levels, points, achievements, and streaks
/// </summary>
public class GamificationService
{
    private const string StorageKey = "gamification_profile";
    private GamificationProfile? _profile;
    private readonly List<Achievement> _allAchievements;
    private readonly SemaphoreSlim _profileLock = new(1, 1);

    public GamificationService()
    {
        _allAchievements = InitializeAchievements();
    }

    /// <summary>
    /// Get or create the user's gamification profile
    /// </summary>
    public async Task<GamificationProfile> GetProfileAsync()
    {
        if (_profile != null)
            return _profile;

        await _profileLock.WaitAsync().ConfigureAwait(false);
        try
        {
            // Double-check pattern
            if (_profile != null)
                return _profile;

            try
            {
                var json = await SecureStorage.GetAsync(StorageKey).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(json))
                {
                    _profile = JsonSerializer.Deserialize<GamificationProfile>(json);
                }
            }
            catch (Exception)
            {
                // If loading fails, create new profile
            }

            _profile ??= new GamificationProfile();
            return _profile;
        }
        finally
        {
            _profileLock.Release();
        }
    }

    /// <summary>
    /// Save the gamification profile to storage
    /// </summary>
    private async Task SaveProfileAsync()
    {
        if (_profile == null)
            return;

        await _profileLock.WaitAsync().ConfigureAwait(false);
        try
        {
            var json = JsonSerializer.Serialize(_profile);
            await SecureStorage.SetAsync(StorageKey, json).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Handle storage errors gracefully
        }
        finally
        {
            _profileLock.Release();
        }
    }

    /// <summary>
    /// Award points to the user and check for level up
    /// </summary>
    public async Task<(bool leveledUp, int newLevel)> AwardPointsAsync(int points)
    {
        var profile = await GetProfileAsync().ConfigureAwait(false);
        var oldLevel = profile.Level;
        
        profile.ExperiencePoints += points;
        profile.TotalPoints += points;

        // Check for level up
        while (profile.ExperiencePoints >= profile.ExperienceToNextLevel)
        {
            profile.ExperiencePoints -= profile.ExperienceToNextLevel;
            profile.Level++;
        }

        await SaveProfileAsync().ConfigureAwait(false);

        var leveledUp = profile.Level > oldLevel;
        return (leveledUp, profile.Level);
    }

    /// <summary>
    /// Record an expense tracking action
    /// </summary>
    public async Task<List<Achievement>> RecordExpenseTrackedAsync()
    {
        var profile = await GetProfileAsync().ConfigureAwait(false);
        profile.TotalExpensesTracked++;
        profile.LastActivityDate = DateTime.UtcNow;

        // Update streak
        await UpdateStreakAsync().ConfigureAwait(false);

        // Award points
        await AwardPointsAsync(PointAction.AddExpense).ConfigureAwait(false);

        // Check and unlock achievements
        var newAchievements = await CheckAchievementsAsync().ConfigureAwait(false);

        await SaveProfileAsync().ConfigureAwait(false);
        return newAchievements;
    }

    /// <summary>
    /// Update the user's tracking streak
    /// </summary>
    private async Task UpdateStreakAsync()
    {
        var profile = await GetProfileAsync().ConfigureAwait(false);
        var today = DateTime.UtcNow.Date;
        var lastActivity = profile.LastActivityDate.Date;

        var daysSinceLastActivity = (today - lastActivity).Days;

        if (daysSinceLastActivity == 0)
        {
            // Same day, no change
            return;
        }
        else if (daysSinceLastActivity == 1)
        {
            // Consecutive day, increment streak
            profile.CurrentStreak++;
            
            if (profile.CurrentStreak > profile.LongestStreak)
            {
                profile.LongestStreak = profile.CurrentStreak;
            }

            // Award streak points only once per milestone
            // Weekly bonus (every 7 days)
            if (profile.CurrentStreak % 7 == 0 && profile.LastWeeklyBonusDay != profile.CurrentStreak)
            {
                await AwardPointsAsync(PointAction.WeeklyStreak).ConfigureAwait(false);
                profile.LastWeeklyBonusDay = profile.CurrentStreak;
            }
            // Monthly bonus (every 30 days)
            if (profile.CurrentStreak % 30 == 0 && profile.LastMonthlyBonusDay != profile.CurrentStreak)
            {
                await AwardPointsAsync(PointAction.MonthlyStreak).ConfigureAwait(false);
                profile.LastMonthlyBonusDay = profile.CurrentStreak;
            }
        }
        else
        {
            // Streak broken
            profile.CurrentStreak = 1;
            profile.LastWeeklyBonusDay = 0; // Reset weekly bonus tracker
            profile.LastMonthlyBonusDay = 0; // Reset monthly bonus tracker
        }

        profile.LastActivityDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Check and unlock any newly earned achievements (without awarding points to prevent recursion)
    /// </summary>
    private async Task<List<Achievement>> CheckAchievementsAsync()
    {
        var profile = await GetProfileAsync().ConfigureAwait(false);
        var newlyUnlocked = new List<Achievement>();

        var lockedAchievements = _allAchievements.Where(a => !profile.UnlockedAchievements.Contains(a.Id));

        foreach (var achievement in lockedAchievements)
        {
            var shouldUnlock = achievement.Id switch
            {
                "first_expense" => profile.TotalExpensesTracked >= 1,
                "expense_novice" => profile.TotalExpensesTracked >= 10,
                "expense_tracker" => profile.TotalExpensesTracked >= 50,
                "expense_master" => profile.TotalExpensesTracked >= 100,
                "week_streak" => profile.CurrentStreak >= 7,
                "month_streak" => profile.CurrentStreak >= 30,
                "streak_legend" => profile.LongestStreak >= 100,
                "level_5" => profile.Level >= 5,
                "level_10" => profile.Level >= 10,
                "level_25" => profile.Level >= 25,
                "point_collector" => profile.TotalPoints >= 1000,
                "point_hoarder" => profile.TotalPoints >= 5000,
                _ => false
            };

            if (shouldUnlock)
            {
                profile.UnlockedAchievements.Add(achievement.Id);
                achievement.IsUnlocked = true;
                achievement.UnlockedDate = DateTime.UtcNow;
                newlyUnlocked.Add(achievement);

                // Award achievement points directly to avoid recursion
                profile.ExperiencePoints += achievement.PointsReward;
                profile.TotalPoints += achievement.PointsReward;

                // Check for level up without triggering another achievement check
                while (profile.ExperiencePoints >= profile.ExperienceToNextLevel && profile.ExperienceToNextLevel > 0)
                {
                    profile.ExperiencePoints -= profile.ExperienceToNextLevel;
                    profile.Level++;
                }
            }
        }

        return newlyUnlocked;
    }

    /// <summary>
    /// Get all achievements with unlock status
    /// </summary>
    public async Task<List<Achievement>> GetAchievementsAsync()
    {
        var profile = await GetProfileAsync().ConfigureAwait(false);
        
        foreach (var achievement in _allAchievements)
        {
            achievement.IsUnlocked = profile.UnlockedAchievements.Contains(achievement.Id);
        }

        return _allAchievements;
    }

    /// <summary>
    /// Calculate budget status for gamification
    /// </summary>
    public BudgetStatus CalculateBudgetStatus(decimal monthlyBudget, decimal currentSpending)
    {
        return new BudgetStatus
        {
            BudgetLimit = monthlyBudget,
            CurrentSpending = currentSpending
        };
    }

    /// <summary>
    /// Calculate budget goal progress using 70-20-10 rule
    /// </summary>
    public BudgetGoalTracker CalculateBudgetGoalProgress(decimal income, decimal essentials, decimal savings, decimal discretionary)
    {
        return new BudgetGoalTracker
        {
            TotalIncome = income,
            EssentialsSpent = essentials,
            SavingsInvested = savings,
            DiscretionarySpent = discretionary
        };
    }

    /// <summary>
    /// Initialize all available achievements
    /// </summary>
    private List<Achievement> InitializeAchievements()
    {
        return new List<Achievement>
        {
            // First steps
            new Achievement
            {
                Id = "first_expense",
                Name = "First Step",
                Description = "Track your first expense",
                Icon = "🎯",
                PointsReward = 25,
                Category = AchievementCategory.Tracking
            },
            new Achievement
            {
                Id = "expense_novice",
                Name = "Expense Novice",
                Description = "Track 10 expenses",
                Icon = "📝",
                PointsReward = 50,
                Category = AchievementCategory.Tracking
            },
            new Achievement
            {
                Id = "expense_tracker",
                Name = "Expense Tracker",
                Description = "Track 50 expenses",
                Icon = "📊",
                PointsReward = 100,
                Category = AchievementCategory.Tracking
            },
            new Achievement
            {
                Id = "expense_master",
                Name = "Expense Master",
                Description = "Track 100 expenses",
                Icon = "👑",
                PointsReward = 250,
                Category = AchievementCategory.Tracking
            },
            
            // Streaks
            new Achievement
            {
                Id = "week_streak",
                Name = "Week Warrior",
                Description = "Track expenses for 7 days in a row",
                Icon = "🔥",
                PointsReward = 100,
                Category = AchievementCategory.Streak
            },
            new Achievement
            {
                Id = "month_streak",
                Name = "Monthly Master",
                Description = "Track expenses for 30 days in a row",
                Icon = "⭐",
                PointsReward = 300,
                Category = AchievementCategory.Streak
            },
            new Achievement
            {
                Id = "streak_legend",
                Name = "Streak Legend",
                Description = "Achieve a 100-day streak",
                Icon = "🏆",
                PointsReward = 1000,
                Category = AchievementCategory.Streak
            },
            
            // Levels
            new Achievement
            {
                Id = "level_5",
                Name = "Rising Star",
                Description = "Reach level 5",
                Icon = "⭐",
                PointsReward = 50,
                Category = AchievementCategory.General
            },
            new Achievement
            {
                Id = "level_10",
                Name = "Skilled Tracker",
                Description = "Reach level 10",
                Icon = "🌟",
                PointsReward = 100,
                Category = AchievementCategory.General
            },
            new Achievement
            {
                Id = "level_25",
                Name = "Finance Guru",
                Description = "Reach level 25",
                Icon = "💎",
                PointsReward = 500,
                Category = AchievementCategory.General
            },
            
            // Points
            new Achievement
            {
                Id = "point_collector",
                Name = "Point Collector",
                Description = "Earn 1000 total points",
                Icon = "💰",
                PointsReward = 100,
                Category = AchievementCategory.General
            },
            new Achievement
            {
                Id = "point_hoarder",
                Name = "Point Hoarder",
                Description = "Earn 5000 total points",
                Icon = "💎",
                PointsReward = 500,
                Category = AchievementCategory.General
            }
        };
    }

    /// <summary>
    /// Reset the gamification profile (for testing or user request)
    /// </summary>
    public async Task ResetProfileAsync()
    {
        _profile = new GamificationProfile();
        await SaveProfileAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Get motivational message based on user's progress (focused on productivity, not gaming)
    /// </summary>
    public async Task<string> GetMotivationalMessageAsync()
    {
        var profile = await GetProfileAsync().ConfigureAwait(false);

        var messages = new List<string>();

        if (profile.CurrentStreak > 0)
        {
            messages.Add($"✅ {profile.CurrentStreak} days of consistent tracking!");
        }

        if (profile.Level >= 10)
        {
            messages.Add($"📊 Building great financial awareness!");
        }

        if (profile.TotalExpensesTracked > 50)
        {
            messages.Add($"💰 {profile.TotalExpensesTracked} expenses tracked - excellent progress!");
        }

        if (messages.Count == 0)
        {
            messages.Add("📈 Track expenses to build better habits!");
        }

        return messages[Random.Shared.Next(messages.Count)];
    }
}
