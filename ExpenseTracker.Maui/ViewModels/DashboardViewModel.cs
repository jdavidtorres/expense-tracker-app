using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Shared.Models;
using ExpenseTracker.Shared.Services;

namespace ExpenseTracker.Maui.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

    public DashboardViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
        Title = "Dashboard";
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
        }
    }

    [RelayCommand]
    private async Task NavigateToSubscriptionsAsync()
    {
        await Shell.Current.GoToAsync("//subscriptions");
    }

    [RelayCommand]
    private async Task NavigateToInvoicesAsync()
    {
        await Shell.Current.GoToAsync("//invoices");
    }

    public string GetMonthName(int month)
    {
        return new DateTime(2024, month, 1).ToString("MMMM");
    }
}