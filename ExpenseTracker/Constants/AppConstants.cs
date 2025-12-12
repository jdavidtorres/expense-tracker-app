namespace ExpenseTracker.Constants;

/// <summary>
/// Navigation route constants used throughout the application
/// </summary>
public static class NavigationRoutes
{
    public const string Dashboard = "dashboard";
    public const string Gamification = "gamification";
    public const string Subscriptions = "subscriptions";
    public const string Invoices = "invoices";
    public const string AddSubscription = "add-subscription";
    public const string EditSubscription = "edit-subscription";
    public const string AddInvoice = "add-invoice";
    public const string EditInvoice = "edit-invoice";
    public const string Debts = "debts";
    public const string AddDebt = "add-debt";
    public const string EditDebt = "edit-debt";
    public const string Incomes = "incomes";
    public const string AddIncome = "add-income";
    public const string EditIncome = "edit-income";
    public const string SavingsGoals = "savings-goals";
    public const string AddSavingsGoal = "add-savings-goal";
    public const string EditSavingsGoal = "edit-savings-goal";
    public const string NavigateBack = "..";
}

/// <summary>
/// Storage key constants for SecureStorage and Preferences
/// </summary>
public static class StorageKeys
{
    public const string GamificationProfile = "gamification_profile";
}

/// <summary>
/// Common error messages used throughout the application
/// </summary>
public static class ErrorMessages
{
    public const string NameRequired = "Name is required.";
    public const string AmountMustBeGreaterThanZero = "Amount must be greater than 0.";
    public const string BillingDateCannotBeInPast = "Next billing date cannot be in the past.";
    public const string LoadFailed = "Failed to load {0}: {1}";
    public const string SaveFailed = "Failed to save {0}: {1}";
    public const string DeleteFailed = "Failed to delete {0}: {1}";
    public const string UpdateFailed = "Failed to update {0}: {1}";
}

/// <summary>
/// Achievement ID constants
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
