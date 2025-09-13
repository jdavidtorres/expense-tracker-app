using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

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

    public DashboardViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [RelayCommand]
    private async Task LoadDashboardDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            // Load monthly summary
            CurrentMonthSummary = await _expenseService.GetMonthlySummaryAsync(DateTime.Now.Year, DateTime.Now.Month);

            // Load yearly summary
            CurrentYearSummary = await _expenseService.GetYearlySummaryAsync(DateTime.Now.Year);

            // Load category breakdown
            var categories = await _expenseService.GetCategorySummaryAsync();
            CategoryBreakdown = new ObservableCollection<CategorySummary>(categories);

            // Load recent subscriptions
            var subscriptions = await _expenseService.GetSubscriptionsAsync();
            RecentSubscriptions = new ObservableCollection<Subscription>(subscriptions.Take(5));

            // Load recent invoices
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
}
