using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Constants;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for the gamification page showing achievements, levels, and progress
/// </summary>
public partial class GamificationViewModel : BaseViewModel
{
    private readonly GamificationService _gamificationService;

    [ObservableProperty]
    private GamificationProfile? profile;

    [ObservableProperty]
    private ObservableCollection<Achievement> achievements = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasRecentAchievements))]
    private ObservableCollection<Achievement> recentAchievements = new();

    [ObservableProperty]
    private string motivationalMessage = string.Empty;

    [ObservableProperty]
    private BudgetStatus? budgetStatus;

    [ObservableProperty]
    private BudgetGoalTracker? budgetGoalTracker;

    /// <summary>
    /// Gets whether there are any recent achievements to display
    /// </summary>
    public bool HasRecentAchievements => RecentAchievements.Count > 0;

    [ObservableProperty]
    private bool showLevelUpCelebration = false;

    [ObservableProperty]
    private bool showAchievementUnlocked = false;

    [ObservableProperty]
    private Achievement? lastUnlockedAchievement;

    public GamificationViewModel(GamificationService gamificationService)
    {
        _gamificationService = gamificationService;
        Title = "Achievements";
    }

    /// <summary>
    /// Loads all gamification data including profile, achievements, and motivational messages
    /// </summary>
    [RelayCommand]
    private async Task LoadGamificationDataAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            Profile = await _gamificationService.GetProfileAsync();
            var allAchievements = await _gamificationService.GetAchievementsAsync();
            Achievements = new ObservableCollection<Achievement>(allAchievements);

            // Get recently unlocked achievements (last 3)
            var recent = allAchievements
                .Where(a => a.IsUnlocked && a.UnlockedDate.HasValue)
                .OrderByDescending(a => a.UnlockedDate)
                .Take(3);
            RecentAchievements = new ObservableCollection<Achievement>(recent);

            MotivationalMessage = await _gamificationService.GetMotivationalMessageAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = string.Format(ErrorMessages.LoadFailed, "gamification data", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Refreshes the gamification data
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadGamificationDataAsync();
    }

    /// <summary>
    /// Updates the budget status display
    /// </summary>
    public void UpdateBudgetStatus(decimal monthlyBudget, decimal currentSpending)
    {
        BudgetStatus = _gamificationService.CalculateBudgetStatus(monthlyBudget, currentSpending);
    }

    /// <summary>
    /// Updates the budget goal progress display
    /// </summary>
    public void UpdateBudgetGoalProgress(decimal income, decimal essentials, decimal savings, decimal discretionary)
    {
        BudgetGoalTracker = _gamificationService.CalculateBudgetGoalProgress(income, essentials, savings, discretionary);
    }

    /// <summary>
    /// Records an expense and returns any newly unlocked achievements
    /// </summary>
    public async Task<List<Achievement>> RecordExpenseTrackedAsync()
    {
        var newAchievements = await _gamificationService.RecordExpenseTrackedAsync();
        
        if (newAchievements.Any())
        {
            LastUnlockedAchievement = newAchievements.First();
            ShowAchievementUnlocked = true;
            
            // Refresh data
            await LoadGamificationDataAsync();
        }

        return newAchievements;
    }

    /// <summary>
    /// Dismisses the achievement notification
    /// </summary>
    [RelayCommand]
    private void DismissAchievementNotification()
    {
        ShowAchievementUnlocked = false;
        LastUnlockedAchievement = null;
    }

    /// <summary>
    /// Dismisses the level up celebration
    /// </summary>
    [RelayCommand]
    private void DismissLevelUpCelebration()
    {
        ShowLevelUpCelebration = false;
    }
}
