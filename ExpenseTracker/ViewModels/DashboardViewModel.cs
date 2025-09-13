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
        Title = "Dashboard";
    }

    [RelayCommand]
    private async Task LoadDashboardDataAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            var currentDate = DateTime.Now;

            // Load current month and year summaries
            var monthlyTask = _expenseService.GetMonthlySummaryAsync(currentDate.Year, currentDate.Month);
            var yearlyTask = _expenseService.GetYearlySummaryAsync(currentDate.Year);
            var categoriesTask = _expenseService.GetCategorySummaryAsync();
            var subscriptionsTask = _expenseService.GetSubscriptionsAsync();
            var invoicesTask = _expenseService.GetInvoicesAsync();

            await Task.WhenAll(monthlyTask, yearlyTask, categoriesTask, subscriptionsTask, invoicesTask);

            CurrentMonthSummary = await monthlyTask;
            CurrentYearSummary = await yearlyTask;

            var categories = await categoriesTask;
            CategoryBreakdown = new ObservableCollection<CategorySummary>(categories);

            var subscriptions = await subscriptionsTask;
            RecentSubscriptions = new ObservableCollection<Subscription>(subscriptions.Take(5));

            var invoices = await invoicesTask;
            RecentInvoices = new ObservableCollection<Invoice>(invoices.OrderByDescending(i => i.IssueDate).Take(5));
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
