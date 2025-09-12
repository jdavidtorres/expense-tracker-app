using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Shared.Models;
using ExpenseTracker.Shared.Services;

namespace ExpenseTracker.Maui.ViewModels;

public partial class SubscriptionsViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

    public SubscriptionsViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
        Title = "Subscriptions";
        Subscriptions = new ObservableCollection<Subscription>();
    }

    [ObservableProperty]
    private ObservableCollection<Subscription> subscriptions;

    [RelayCommand]
    private async Task LoadSubscriptionsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            var subscriptions = await _expenseService.GetSubscriptionsAsync();
            
            Subscriptions.Clear();
            foreach (var subscription in subscriptions)
            {
                Subscriptions.Add(subscription);
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load subscriptions: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}