using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Constants;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for the dashboard page showing expense summaries and recent items
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

    public DashboardViewModel(ExpenseService expenseService, GamificationService gamificationService)
    {
        _expenseService = expenseService;
        _gamificationService = gamificationService;
    }

    /// <summary>
    /// Loads all dashboard data including summaries, recent items, and gamification profile
    /// </summary>
    [RelayCommand]
    private async Task LoadDashboardDataAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            var now = DateTime.Now;

            // Load all data in parallel for better performance
            var profileTask = _gamificationService.GetProfileAsync();
            var messageTask = _gamificationService.GetMotivationalMessageAsync();
            var monthlyTask = _expenseService.GetMonthlySummaryAsync(now.Year, now.Month);
            var yearlyTask = _expenseService.GetYearlySummaryAsync(now.Year);
            var categoriesTask = _expenseService.GetCategorySummaryAsync();
            var subscriptionsTask = _expenseService.GetSubscriptionsAsync();
            var invoicesTask = _expenseService.GetInvoicesAsync();

            await Task.WhenAll(
                profileTask,
                messageTask,
                monthlyTask,
                yearlyTask,
                categoriesTask,
                subscriptionsTask,
                invoicesTask
            );

            // Update UI with loaded data
            GamificationProfile = await profileTask;
            MotivationalMessage = await messageTask;
            CurrentMonthSummary = await monthlyTask;
            CurrentYearSummary = await yearlyTask;
            CategoryBreakdown = new ObservableCollection<CategorySummary>(await categoriesTask);
            RecentSubscriptions = new ObservableCollection<Subscription>((await subscriptionsTask).Take(5));
            RecentInvoices = new ObservableCollection<Invoice>((await invoicesTask).Take(5));
        }
        catch (Exception ex)
        {
            ErrorMessage = string.Format(ErrorMessages.LoadFailed, "dashboard data", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Navigates to the subscriptions page
    /// </summary>
    [RelayCommand]
    private async Task NavigateToSubscriptionsAsync()
    {
        await Shell.Current.GoToAsync(NavigationRoutes.Subscriptions);
    }

    /// <summary>
    /// Navigates to the invoices page
    /// </summary>
    [RelayCommand]
    private async Task NavigateToInvoicesAsync()
    {
        await Shell.Current.GoToAsync(NavigationRoutes.Invoices);
    }

    /// <summary>
    /// Refreshes the dashboard data
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDashboardDataAsync();
    }

    /// <summary>
    /// Navigates to the gamification page
    /// </summary>
    [RelayCommand]
    private async Task NavigateToGamificationAsync()
    {
        await Shell.Current.GoToAsync(NavigationRoutes.Gamification);
    }
}
