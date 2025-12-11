namespace ExpenseTracker.Constants;

/// <summary>
/// Constants for gamification system including points, achievements, and storage keys
/// </summary>
public static class GamificationConstants
{
    /// <summary>
    /// Storage key for gamification profile
    /// </summary>
    public const string ProfileStorageKey = "gamification_profile";

    /// <summary>
    /// Point values for various actions
    /// </summary>
    public static class Points
    {
        public const int AddExpense = 10;
        public const int WeeklyStreak = 50;
        public const int MonthlyStreak = 200;
    }

    /// <summary>
    /// Streak milestone days
    /// </summary>
    public static class StreakMilestones
    {
        public const int WeeklyDays = 7;
        public const int MonthlyDays = 30;
        public const int LegendaryDays = 100;
    }

    /// <summary>
    /// Achievement identifiers
    /// </summary>
    public static class AchievementIds
    {
        public const string FirstExpense = "first_expense";
        public const string ExpenseNovice = "expense_novice";
        public const string ExpenseTracker = "expense_tracker";
        public const string ExpenseMaster = "expense_master";
        public const string WeekStreak = "week_streak";
        public const string MonthStreak = "month_streak";
        public const string StreakLegend = "streak_legend";
        public const string Level5 = "level_5";
        public const string Level10 = "level_10";
        public const string Level25 = "level_25";
        public const string PointCollector = "point_collector";
        public const string PointHoarder = "point_hoarder";
    }

    /// <summary>
    /// Point thresholds for achievements
    /// </summary>
    public static class PointThresholds
    {
        public const int Collector = 1000;
        public const int Hoarder = 5000;
    }

    /// <summary>
    /// Expense tracking thresholds for achievements
    /// </summary>
    public static class ExpenseThresholds
    {
        public const int First = 1;
        public const int Novice = 10;
        public const int Tracker = 50;
        public const int Master = 100;
    }

    /// <summary>
    /// Level thresholds for achievements
    /// </summary>
    public static class LevelThresholds
    {
        public const int RisingStar = 5;
        public const int SkilledTracker = 10;
        public const int FinanceGuru = 25;
    }
}
