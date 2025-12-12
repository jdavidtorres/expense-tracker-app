using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for the dashboard displaying summary data and quick access
/// </summary>
public partial class DashboardViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;
    private readonly GamificationService _gamificationService;

    [ObservableProperty]
    private MonthlySummary? currentMonthSummary;

    [ObservableProperty]
    private YearlySummary? currentYearSummary;

    [ObservableProperty]
    private ObservableCollection<CategorySummary> categoryBreakdown = new();

    [ObservableProperty]
    private ObservableCollection<Subscription> recentSubscriptions = new();

    [ObservableProperty]
    private ObservableCollection<Invoice> recentInvoices = new();

    [ObservableProperty]
    private GamificationProfile? gamificationProfile;

    [ObservableProperty]
    private string motivationalMessage = string.Empty;

    [ObservableProperty]
    private string budgetHealthMessage = string.Empty;

    [ObservableProperty]
    private Color budgetHealthColor = Colors.Gray;

    [ObservableProperty]
    private bool isLevelUp;

    public DashboardViewModel(ExpenseService expenseService, GamificationService gamificationService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
        _gamificationService = gamificationService ?? throw new ArgumentNullException(nameof(gamificationService));
    }

    [RelayCommand]
    private async Task LoadDashboardDataAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            var now = DateTime.Now;

            // Load gamification profile
            GamificationProfile = await _gamificationService.GetProfileAsync();
            MotivationalMessage = await _gamificationService.GetMotivationalMessageAsync();

            // Load monthly summary
            CurrentMonthSummary = await _expenseService.GetMonthlySummaryAsync(now.Year, now.Month);

            // Calculate budget health
            if (CurrentMonthSummary != null)
            {
                // Assuming a fixed budget for now, or fetch from settings if available
                decimal monthlyBudget = 2000m; // TODO: make configurable
                var budgetStatus = _gamificationService.CalculateBudgetStatus(monthlyBudget, CurrentMonthSummary.TotalExpenses);
                BudgetHealthMessage = budgetStatus.StatusMessage;
                BudgetHealthColor = Color.FromArgb(budgetStatus.StatusColor);
            }

            // Load yearly summary
            CurrentYearSummary = await _expenseService.GetYearlySummaryAsync(now.Year);

            // Load category breakdown
            var categories = await _expenseService.GetCategorySummaryAsync();
            CategoryBreakdown = new ObservableCollection<CategorySummary>(categories);

            // Load recent subscriptions (top 5)
            var subscriptions = await _expenseService.GetSubscriptionsAsync();
            RecentSubscriptions = new ObservableCollection<Subscription>(subscriptions.Take(5));

            // Load recent invoices (top 5)
            var invoices = await _expenseService.GetInvoicesAsync();
            RecentInvoices = new ObservableCollection<Invoice>(invoices.Take(5));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load dashboard data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToSubscriptionsAsync()
    {
        await Shell.Current.GoToAsync("subscriptions");
    }

    [RelayCommand]
    private async Task NavigateToInvoicesAsync()
    {
        await Shell.Current.GoToAsync("invoices");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDashboardDataAsync();
    }

    [RelayCommand]
    private async Task NavigateToGamificationAsync()
    {
        await Shell.Current.GoToAsync("gamification");
    }

    // Helper to trigger confetti from service (subscribe to events in a real app)
    public void TriggerLevelUp()
    {
        IsLevelUp = true;
        // Reset after delay handled by view
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(3000);
            IsLevelUp = false;
        });
    }
}
