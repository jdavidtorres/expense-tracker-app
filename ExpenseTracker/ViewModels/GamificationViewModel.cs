using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class GamificationViewModel : BaseViewModel
{
    private readonly GamificationService _gamificationService;

    [ObservableProperty]
    private GamificationProfile? profile;

    [ObservableProperty]
    private ObservableCollection<Achievement> achievements = new();

    [ObservableProperty]
    private ObservableCollection<Achievement> recentAchievements = new();

    [ObservableProperty]
    private string motivationalMessage = string.Empty;

    [ObservableProperty]
    private BudgetStatus? budgetStatus;

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

    [RelayCommand]
    private async Task LoadGamificationDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

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
            
            // Notify property change for HasRecentAchievements
            OnPropertyChanged(nameof(HasRecentAchievements));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load gamification data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadGamificationDataAsync();
    }

    public async Task UpdateBudgetStatusAsync(decimal monthlyBudget, decimal currentSpending)
    {
        BudgetStatus = _gamificationService.CalculateBudgetStatus(monthlyBudget, currentSpending);
    }

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

    [RelayCommand]
    private void DismissAchievementNotification()
    {
        ShowAchievementUnlocked = false;
        LastUnlockedAchievement = null;
    }

    [RelayCommand]
    private void DismissLevelUpCelebration()
    {
        ShowLevelUpCelebration = false;
    }
}
