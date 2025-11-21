using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

/// <summary>
/// Represents user's gamification profile including level, points, and achievements
/// </summary>
public class GamificationProfile
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = "default-user";

    [JsonPropertyName("level")]
    public int Level { get; set; } = 1;

    [JsonPropertyName("experiencePoints")]
    public int ExperiencePoints { get; set; } = 0;

    [JsonPropertyName("experienceToNextLevel")]
    public int ExperienceToNextLevel => Level * 100;

    [JsonPropertyName("totalExpensesTracked")]
    public int TotalExpensesTracked { get; set; } = 0;

    [JsonPropertyName("currentStreak")]
    public int CurrentStreak { get; set; } = 0;

    [JsonPropertyName("longestStreak")]
    public int LongestStreak { get; set; } = 0;

    [JsonPropertyName("lastActivityDate")]
    public DateTime LastActivityDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("unlockedAchievements")]
    public List<string> UnlockedAchievements { get; set; } = new();

    [JsonPropertyName("totalPoints")]
    public int TotalPoints { get; set; } = 0;

    /// <summary>
    /// Calculate level progress percentage
    /// </summary>
    public double LevelProgressPercentage => (double)ExperiencePoints / ExperienceToNextLevel * 100;
}

/// <summary>
/// Represents an achievement/badge that can be unlocked
/// </summary>
public class Achievement
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = "🏆";

    [JsonPropertyName("pointsReward")]
    public int PointsReward { get; set; } = 0;

    [JsonPropertyName("isUnlocked")]
    public bool IsUnlocked { get; set; } = false;

    [JsonPropertyName("unlockedDate")]
    public DateTime? UnlockedDate { get; set; }

    [JsonPropertyName("category")]
    public AchievementCategory Category { get; set; } = AchievementCategory.General;
}

public enum AchievementCategory
{
    General,
    Tracking,
    Streak,
    Budget,
    Savings
}

/// <summary>
/// Represents a point-earning action
/// </summary>
public class PointAction
{
    public const int AddExpense = 10;
    public const int DailyLogin = 5;
    public const int WeeklyStreak = 50;
    public const int MonthlyStreak = 200;
    public const int StayUnderBudget = 100;
    public const int CompleteProfile = 25;
    public const int AddFirstSubscription = 15;
    public const int AddFirstInvoice = 15;
}

/// <summary>
/// Represents budget status for gamification
/// </summary>
public class BudgetStatus
{
    public decimal BudgetLimit { get; set; }
    public decimal CurrentSpending { get; set; }
    public decimal RemainingBudget => BudgetLimit - CurrentSpending;
    public double PercentageUsed => BudgetLimit > 0 ? (double)(CurrentSpending / BudgetLimit * 100) : 0;
    
    public BudgetHealthLevel HealthLevel
    {
        get
        {
            if (PercentageUsed <= 50) return BudgetHealthLevel.Excellent;
            if (PercentageUsed <= 75) return BudgetHealthLevel.Good;
            if (PercentageUsed <= 90) return BudgetHealthLevel.Warning;
            if (PercentageUsed <= 100) return BudgetHealthLevel.Critical;
            return BudgetHealthLevel.OverBudget;
        }
    }

    public string StatusColor
    {
        get
        {
            return HealthLevel switch
            {
                BudgetHealthLevel.Excellent => "#4CAF50",
                BudgetHealthLevel.Good => "#8BC34A",
                BudgetHealthLevel.Warning => "#FFC107",
                BudgetHealthLevel.Critical => "#FF9800",
                BudgetHealthLevel.OverBudget => "#F44336",
                _ => "#9E9E9E"
            };
        }
    }

    public string StatusMessage
    {
        get
        {
            return HealthLevel switch
            {
                BudgetHealthLevel.Excellent => "Amazing! You're doing great! 🎉",
                BudgetHealthLevel.Good => "Good job! Keep it up! 👍",
                BudgetHealthLevel.Warning => "Watch your spending! ⚠️",
                BudgetHealthLevel.Critical => "Almost at your limit! 🚨",
                BudgetHealthLevel.OverBudget => "Over budget! Time to review! ❌",
                _ => "Set a budget to track progress"
            };
        }
    }
}

public enum BudgetHealthLevel
{
    Excellent,
    Good,
    Warning,
    Critical,
    OverBudget
}
