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

        try
        {
            var json = await SecureStorage.GetAsync(StorageKey);
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
    private async Task SaveProfileAsync()
    {
        if (_profile == null)
            return;

        try
        {
            var json = JsonSerializer.Serialize(_profile);
            await SecureStorage.SetAsync(StorageKey, json);
        }
        catch (Exception)
        {
            // Handle storage errors gracefully
        }
    }

    /// <summary>
    /// Award points to the user and check for level up
    /// </summary>
    public async Task<(bool leveledUp, int newLevel)> AwardPointsAsync(int points)
    {
        var profile = await GetProfileAsync();
        var oldLevel = profile.Level;
        
        profile.ExperiencePoints += points;
        profile.TotalPoints += points;

        // Check for level up
        while (profile.ExperiencePoints >= profile.ExperienceToNextLevel)
        {
            profile.ExperiencePoints -= profile.ExperienceToNextLevel;
            profile.Level++;
        }

        await SaveProfileAsync();

        var leveledUp = profile.Level > oldLevel;
        return (leveledUp, profile.Level);
    }

    /// <summary>
    /// Record an expense tracking action
    /// </summary>
    public async Task<List<Achievement>> RecordExpenseTrackedAsync()
    {
        var profile = await GetProfileAsync();
        profile.TotalExpensesTracked++;
        profile.LastActivityDate = DateTime.UtcNow;

        // Update streak
        await UpdateStreakAsync();

        // Award points
        await AwardPointsAsync(PointAction.AddExpense);

        // Check and unlock achievements
        var newAchievements = await CheckAchievementsAsync();

        await SaveProfileAsync();
        return newAchievements;
    }

    /// <summary>
    /// Update the user's tracking streak
    /// </summary>
    private async Task UpdateStreakAsync()
    {
        var profile = await GetProfileAsync();
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

            // Award streak points
            if (profile.CurrentStreak % 7 == 0)
            {
                await AwardPointsAsync(PointAction.WeeklyStreak);
            }
            if (profile.CurrentStreak % 30 == 0)
            {
                await AwardPointsAsync(PointAction.MonthlyStreak);
            }
        }
        else
        {
            // Streak broken
            profile.CurrentStreak = 1;
        }

        profile.LastActivityDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Check and unlock any newly earned achievements
    /// </summary>
    private async Task<List<Achievement>> CheckAchievementsAsync()
    {
        var profile = await GetProfileAsync();
        var newlyUnlocked = new List<Achievement>();

        foreach (var achievement in _allAchievements)
        {
            if (profile.UnlockedAchievements.Contains(achievement.Id))
                continue;

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

                // Award achievement points
                await AwardPointsAsync(achievement.PointsReward);
            }
        }

        return newlyUnlocked;
    }

    /// <summary>
    /// Get all achievements with unlock status
    /// </summary>
    public async Task<List<Achievement>> GetAchievementsAsync()
    {
        var profile = await GetProfileAsync();
        
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
        await SaveProfileAsync();
    }

    /// <summary>
    /// Get motivational message based on user's progress
    /// </summary>
    public async Task<string> GetMotivationalMessageAsync()
    {
        var profile = await GetProfileAsync();

        var messages = new List<string>();

        if (profile.CurrentStreak > 0)
        {
            messages.Add($"🔥 {profile.CurrentStreak} day streak! Keep it up!");
        }

        if (profile.Level >= 10)
        {
            messages.Add($"🌟 Level {profile.Level}! You're a finance pro!");
        }

        if (profile.TotalExpensesTracked > 50)
        {
            messages.Add($"📊 {profile.TotalExpensesTracked} expenses tracked! Amazing!");
        }

        if (messages.Count == 0)
        {
            messages.Add("💪 Keep tracking to level up!");
        }

        var random = new Random();
        return messages[random.Next(messages.Count)];
    }
}
