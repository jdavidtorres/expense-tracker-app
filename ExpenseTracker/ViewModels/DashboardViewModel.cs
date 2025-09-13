using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

<<<<<<< HEAD
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

=======
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
    public DashboardViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
        Title = "Dashboard";
<<<<<<< HEAD
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
=======
        Subscriptions = new ObservableCollection<Subscription>();
        Invoices = new ObservableCollection<Invoice>();
    }

    [ObservableProperty]
    private ObservableCollection<Subscription> subscriptions;

    [ObservableProperty]
    private ObservableCollection<Invoice> invoices;

    [ObservableProperty]
    private ExpensesSummary currentSummary = new();

    // Properties that expose backend-calculated values
    public decimal TotalExpenses => CurrentSummary?.Total ?? 0;
    public decimal MonthlyAverage => CurrentSummary?.MonthlyAverage ?? 0;
    public decimal YearlyTotal => CurrentSummary?.YearlyTotal ?? 0;

    [ObservableProperty]
    private int currentMonth = DateTime.Now.Month;

    [ObservableProperty]
    private int currentYear = DateTime.Now.Year;



    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            var subscriptionsTask = _expenseService.GetSubscriptionsAsync();
            var invoicesTask = _expenseService.GetInvoicesAsync();
            var summaryTask = _expenseService.GetMonthlySummaryAsync(CurrentYear, CurrentMonth);

            await Task.WhenAll(subscriptionsTask, invoicesTask, summaryTask);

            Subscriptions.Clear();
            foreach (var subscription in await subscriptionsTask)
            {
                Subscriptions.Add(subscription);
            }

            Invoices.Clear();
            foreach (var invoice in await invoicesTask)
            {
                Invoices.Add(invoice);
            }

            CurrentSummary = await summaryTask;

            // Notify that backend-calculated properties have changed
            OnPropertyChanged(nameof(TotalExpenses));
            OnPropertyChanged(nameof(MonthlyAverage));
            OnPropertyChanged(nameof(YearlyTotal));
        }
        catch (Exception ex)
        {
            SetError($"Failed to load data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
        }
    }

    [RelayCommand]
    private async Task NavigateToSubscriptionsAsync()
    {
<<<<<<< HEAD
        await Shell.Current.GoToAsync("subscriptions");
=======
        await Shell.Current.GoToAsync("//subscriptions");
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
    }

    [RelayCommand]
    private async Task NavigateToInvoicesAsync()
    {
<<<<<<< HEAD
        await Shell.Current.GoToAsync("invoices");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDashboardDataAsync();
    }
}
=======
        await Shell.Current.GoToAsync("//invoices");
    }

    public string GetMonthName(int month)
    {
        return new DateTime(2024, month, 1).ToString("MMMM");
    }
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
