using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class SubscriptionsViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<Subscription> subscriptions = new();

    [ObservableProperty]
    private Subscription? selectedSubscription;

    public SubscriptionsViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [RelayCommand]
    private async Task LoadSubscriptionsAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var data = await _expenseService.GetSubscriptionsAsync();
            Subscriptions = new ObservableCollection<Subscription>(data);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load subscriptions: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteSubscriptionByIdAsync(string id)
    {
        try
        {
            await _expenseService.DeleteSubscriptionAsync(id);
            await LoadSubscriptionsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete subscription: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task DeleteSubscriptionAsync(Subscription subscription)
    {
        try
        {
            await _expenseService.DeleteSubscriptionAsync(subscription.Id);
            await LoadSubscriptionsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete subscription: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task NavigateToAddSubscriptionAsync()
    {
        await Shell.Current.GoToAsync("add-subscription");
    }

    [RelayCommand]
    private async Task NavigateToEditSubscriptionAsync(Subscription subscription)
    {
        await Shell.Current.GoToAsync($"edit-subscription?id={subscription.Id}");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadSubscriptionsAsync();
    }

    [RelayCommand]
    private async Task ToggleSubscriptionStatusAsync(Subscription subscription)
    {
        try
        {
            // Toggle the status (for subscription, this might be active/inactive)
            // You can implement actual status toggle logic here
            await LoadSubscriptionsAsync(); // Refresh to reflect changes
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to toggle subscription status: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task EditSubscriptionAsync(Subscription subscription)
    {
        await Shell.Current.GoToAsync($"edit-subscription?id={subscription.Id}");
    }

    [RelayCommand]
    private async Task AddSubscriptionAsync()
    {
        await Shell.Current.GoToAsync("add-subscription");
    }
}
