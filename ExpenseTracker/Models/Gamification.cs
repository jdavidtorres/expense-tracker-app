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

    [JsonPropertyName("lastWeeklyBonusDay")]
    public int LastWeeklyBonusDay { get; set; } = 0;

    [JsonPropertyName("lastMonthlyBonusDay")]
    public int LastMonthlyBonusDay { get; set; } = 0;

    [JsonPropertyName("unlockedAchievements")]
    public List<string> UnlockedAchievements { get; set; } = new();

    [JsonPropertyName("totalPoints")]
    public int TotalPoints { get; set; } = 0;

    /// <summary>
    /// Calculate level progress percentage (0.0 to 1.0 for ProgressBar)
    /// </summary>
    public double LevelProgressPercentage
    {
        get
        {
            if (ExperienceToNextLevel == 0 || Level == 0)
                return 0;
            return (double)ExperiencePoints / ExperienceToNextLevel;
        }
    }

    /// <summary>
    /// Calculate level progress as a display percentage (0 to 100)
    /// </summary>
    public double LevelProgressDisplayPercentage => LevelProgressPercentage * 100;
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
                BudgetHealthLevel.Excellent => "🛡️ PERFECT DEFENSE! BUDGET UNTOUCHED!",
                BudgetHealthLevel.Good => "⚔️ HOLDING THE LINE! KEEP IT UP!",
                BudgetHealthLevel.Warning => "⚠️ SHIELDS FAILING! WATCH YOUR SPENDING!",
                BudgetHealthLevel.Critical => "🚨 CRITICAL HIT! BUDGET NEARLY DEPLETED!",
                BudgetHealthLevel.OverBudget => "💀 GAME OVER... FOR THIS BUDGET! RETRY NEXT MONTH!",
                _ => "⚔️ START YOUR QUEST: SET A BUDGET!"
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

/// <summary>
/// Represents budget allocation tracking based on financial planning rules (e.g., 70-20-10 rule)
/// </summary>
public class BudgetGoalTracker
{
    [JsonPropertyName("totalIncome")]
    public decimal TotalIncome { get; set; }

    [JsonPropertyName("essentialsSpent")]
    public decimal EssentialsSpent { get; set; }

    [JsonPropertyName("savingsInvested")]
    public decimal SavingsInvested { get; set; }

    [JsonPropertyName("discretionarySpent")]
    public decimal DiscretionarySpent { get; set; }

    // 70-20-10 Rule: 70% Essentials, 20% Savings, 10% Discretionary
    public decimal EssentialsTarget => TotalIncome * 0.70m;
    public decimal SavingsTarget => TotalIncome * 0.20m;
    public decimal DiscretionaryTarget => TotalIncome * 0.10m;

    // Display percentages (0-100)
    public double EssentialsProgress => TotalIncome > 0 && EssentialsTarget > 0 ? (double)(EssentialsSpent / EssentialsTarget * 100) : 0;
    public double SavingsProgress => TotalIncome > 0 && SavingsTarget > 0 ? (double)(SavingsInvested / SavingsTarget * 100) : 0;
    public double DiscretionaryProgress => TotalIncome > 0 && DiscretionaryTarget > 0 ? (double)(DiscretionarySpent / DiscretionaryTarget * 100) : 0;

    // Normalized values for MAUI ProgressBar (0.0 - 1.0)
    public double EssentialsProgressBar => TotalIncome > 0 && EssentialsTarget > 0 ? Math.Min(1.0, (double)(EssentialsSpent / EssentialsTarget)) : 0.0;
    public double SavingsProgressBar => TotalIncome > 0 && SavingsTarget > 0 ? Math.Min(1.0, (double)(SavingsInvested / SavingsTarget)) : 0.0;
    public double DiscretionaryProgressBar => TotalIncome > 0 && DiscretionaryTarget > 0 ? Math.Min(1.0, (double)(DiscretionarySpent / DiscretionaryTarget)) : 0.0;

    public string ProgressMessage
    {
        get
        {
            if (TotalIncome == 0)
                return "Set your income to track budget goals";

            var messages = new List<string>();

            if (SavingsProgress >= 100)
                messages.Add("� TREASURE CHEST SECURED! SAVINGS GOAL MET!");
            else if (SavingsProgress >= 75)
                messages.Add($"� SO CLOSE TO THE LOOT! {SavingsProgress:F0}% SAVED!");
            else if (SavingsProgress >= 50)
                messages.Add($"⚔️ HALFWAY TO GLORY! {SavingsProgress:F0}% SAVED!");
            else
                messages.Add($"�️ BUILD YOUR DEFENSES! {SavingsProgress:F0}% SAVED");

            if (EssentialsProgress > 100)
                messages.Add("⚠️ MANA LOW! ESSENTIALS OVERLOAD!");
            else if (EssentialsProgress > 90)
                messages.Add("� BOSS FIGHT IMMINENT! ESSENTIALS LIMIT NEAR!");

            if (DiscretionaryProgress > 100)
                messages.Add("💣 DAMAGE TAKEN! DISCRETIONARY OVERLOAD!");

            return messages.Count > 0 ? string.Join(" • ", messages) : "✅ QUEST ON TRACK!";
        }
    }
}
