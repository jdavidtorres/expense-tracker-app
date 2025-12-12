using ExpenseTracker.Constants;
using ExpenseTracker.Models;
using System.Text.Json;

namespace ExpenseTracker.Services;

/// <summary>
/// Service for managing gamification features including levels, points, achievements, and streaks
/// </summary>
public class GamificationService
{
    private GamificationProfile? _profile;
    private readonly List<Achievement> _allAchievements;

    public GamificationService()
    {
        _allAchievements = InitializeAchievements();
    }

    /// <summary>
    /// Get or create the user's gamification profile
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The user's gamification profile</returns>
    public async Task<GamificationProfile> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        if (_profile != null)
            return _profile;

        try
        {
            var json = await SecureStorage.GetAsync(GamificationConstants.ProfileStorageKey).ConfigureAwait(false);
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

    /// <summary>
    /// Save the gamification profile to storage
    /// </summary>
    private async Task SaveProfileAsync(CancellationToken cancellationToken = default)
    {
        if (_profile == null)
            return;

        try
        {
            var json = JsonSerializer.Serialize(_profile);
            await SecureStorage.SetAsync(GamificationConstants.ProfileStorageKey, json).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Handle storage errors gracefully
        }
    }

    /// <summary>
    /// Award points to the user and check for level up
    /// </summary>
    /// <param name="points">Number of points to award</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Tuple indicating if user leveled up and the new level</returns>
    public async Task<(bool leveledUp, int newLevel)> AwardPointsAsync(int points, CancellationToken cancellationToken = default)
    {
        var profile = await GetProfileAsync(cancellationToken).ConfigureAwait(false);
        var oldLevel = profile.Level;
        
        profile.ExperiencePoints += points;
        profile.TotalPoints += points;

        // Check for level up
        while (profile.ExperiencePoints >= profile.ExperienceToNextLevel)
        {
            profile.ExperiencePoints -= profile.ExperienceToNextLevel;
            profile.Level++;
        }

        await SaveProfileAsync(cancellationToken).ConfigureAwait(false);

        var leveledUp = profile.Level > oldLevel;
        return (leveledUp, profile.Level);
    }

    /// <summary>
    /// Record an expense tracking action
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>List of newly unlocked achievements</returns>
    public async Task<List<Achievement>> RecordExpenseTrackedAsync(CancellationToken cancellationToken = default)
    {
        var profile = await GetProfileAsync(cancellationToken).ConfigureAwait(false);
        profile.TotalExpensesTracked++;
        profile.LastActivityDate = DateTime.UtcNow;

        // Update streak
        await UpdateStreakAsync(cancellationToken).ConfigureAwait(false);

        // Award points
        await AwardPointsAsync(GamificationConstants.Points.AddExpense, cancellationToken).ConfigureAwait(false);

        // Check and unlock achievements
        var newAchievements = await CheckAchievementsAsync(cancellationToken).ConfigureAwait(false);

        await SaveProfileAsync(cancellationToken).ConfigureAwait(false);
        return newAchievements;
    }

    /// <summary>
    /// Update the user's tracking streak
    /// </summary>
    private async Task UpdateStreakAsync(CancellationToken cancellationToken = default)
    {
        var profile = await GetProfileAsync(cancellationToken).ConfigureAwait(false);
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
            if (profile.CurrentStreak % GamificationConstants.StreakMilestones.WeeklyDays == 0 
                && profile.LastWeeklyBonusDay != profile.CurrentStreak)
            {
                await AwardPointsAsync(GamificationConstants.Points.WeeklyStreak, cancellationToken).ConfigureAwait(false);
                profile.LastWeeklyBonusDay = profile.CurrentStreak;
            }
            // Monthly bonus (every 30 days)
            if (profile.CurrentStreak % GamificationConstants.StreakMilestones.MonthlyDays == 0 
                && profile.LastMonthlyBonusDay != profile.CurrentStreak)
            {
                await AwardPointsAsync(GamificationConstants.Points.MonthlyStreak, cancellationToken).ConfigureAwait(false);
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
    private async Task<List<Achievement>> CheckAchievementsAsync(CancellationToken cancellationToken = default)
    {
        var profile = await GetProfileAsync(cancellationToken).ConfigureAwait(false);
        var newlyUnlocked = new List<Achievement>();

        var lockedAchievements = _allAchievements.Where(a => !profile.UnlockedAchievements.Contains(a.Id));

        foreach (var achievement in lockedAchievements)
        {
            var shouldUnlock = achievement.Id switch
            {
                GamificationConstants.AchievementIds.FirstExpense => profile.TotalExpensesTracked >= GamificationConstants.ExpenseThresholds.First,
                GamificationConstants.AchievementIds.ExpenseNovice => profile.TotalExpensesTracked >= GamificationConstants.ExpenseThresholds.Novice,
                GamificationConstants.AchievementIds.ExpenseTracker => profile.TotalExpensesTracked >= GamificationConstants.ExpenseThresholds.Tracker,
                GamificationConstants.AchievementIds.ExpenseMaster => profile.TotalExpensesTracked >= GamificationConstants.ExpenseThresholds.Master,
                GamificationConstants.AchievementIds.WeekStreak => profile.CurrentStreak >= GamificationConstants.StreakMilestones.WeeklyDays,
                GamificationConstants.AchievementIds.MonthStreak => profile.CurrentStreak >= GamificationConstants.StreakMilestones.MonthlyDays,
                GamificationConstants.AchievementIds.StreakLegend => profile.LongestStreak >= GamificationConstants.StreakMilestones.LegendaryDays,
                GamificationConstants.AchievementIds.Level5 => profile.Level >= GamificationConstants.LevelThresholds.RisingStar,
                GamificationConstants.AchievementIds.Level10 => profile.Level >= GamificationConstants.LevelThresholds.SkilledTracker,
                GamificationConstants.AchievementIds.Level25 => profile.Level >= GamificationConstants.LevelThresholds.FinanceGuru,
                GamificationConstants.AchievementIds.PointCollector => profile.TotalPoints >= GamificationConstants.PointThresholds.Collector,
                GamificationConstants.AchievementIds.PointHoarder => profile.TotalPoints >= GamificationConstants.PointThresholds.Hoarder,
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
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>List of all achievements</returns>
    public async Task<List<Achievement>> GetAchievementsAsync(CancellationToken cancellationToken = default)
    {
        var profile = await GetProfileAsync(cancellationToken).ConfigureAwait(false);
        
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
                Id = GamificationConstants.AchievementIds.FirstExpense,
                Name = "First Step",
                Description = "Track your first expense",
                Icon = "🎯",
                PointsReward = 25,
                Category = AchievementCategory.Tracking
            },
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.ExpenseNovice,
                Name = "Expense Novice",
                Description = "Track 10 expenses",
                Icon = "📝",
                PointsReward = 50,
                Category = AchievementCategory.Tracking
            },
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.ExpenseTracker,
                Name = "Expense Tracker",
                Description = "Track 50 expenses",
                Icon = "📊",
                PointsReward = 100,
                Category = AchievementCategory.Tracking
            },
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.ExpenseMaster,
                Name = "Expense Master",
                Description = "Track 100 expenses",
                Icon = "👑",
                PointsReward = 250,
                Category = AchievementCategory.Tracking
            },
            
            // Streaks
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.WeekStreak,
                Name = "Week Warrior",
                Description = "Track expenses for 7 days in a row",
                Icon = "🔥",
                PointsReward = 100,
                Category = AchievementCategory.Streak
            },
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.MonthStreak,
                Name = "Monthly Master",
                Description = "Track expenses for 30 days in a row",
                Icon = "⭐",
                PointsReward = 300,
                Category = AchievementCategory.Streak
            },
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.StreakLegend,
                Name = "Streak Legend",
                Description = "Achieve a 100-day streak",
                Icon = "🏆",
                PointsReward = 1000,
                Category = AchievementCategory.Streak
            },
            
            // Levels
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.Level5,
                Name = "Rising Star",
                Description = "Reach level 5",
                Icon = "⭐",
                PointsReward = 50,
                Category = AchievementCategory.General
            },
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.Level10,
                Name = "Skilled Tracker",
                Description = "Reach level 10",
                Icon = "🌟",
                PointsReward = 100,
                Category = AchievementCategory.General
            },
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.Level25,
                Name = "Finance Guru",
                Description = "Reach level 25",
                Icon = "💎",
                PointsReward = 500,
                Category = AchievementCategory.General
            },
            
            // Points
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.PointCollector,
                Name = "Point Collector",
                Description = "Earn 1000 total points",
                Icon = "💰",
                PointsReward = 100,
                Category = AchievementCategory.General
            },
            new Achievement
            {
                Id = GamificationConstants.AchievementIds.PointHoarder,
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
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    public async Task ResetProfileAsync(CancellationToken cancellationToken = default)
    {
        _profile = new GamificationProfile();
        await SaveProfileAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Get motivational message based on user's progress (focused on productivity, not gaming)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Motivational message</returns>
    public async Task<string> GetMotivationalMessageAsync(CancellationToken cancellationToken = default)
    {
        var profile = await GetProfileAsync(cancellationToken).ConfigureAwait(false);

        var messages = new List<string>();

        if (profile.CurrentStreak > 0)
        {
            messages.Add($"✅ {profile.CurrentStreak} days of consistent tracking!");
        }

        if (profile.Level >= GamificationConstants.LevelThresholds.SkilledTracker)
        {
            messages.Add($"📊 Building great financial awareness!");
        }

        if (profile.TotalExpensesTracked > GamificationConstants.ExpenseThresholds.Tracker)
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
