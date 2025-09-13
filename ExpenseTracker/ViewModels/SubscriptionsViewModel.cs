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
        Title = "Subscriptions";
    }

    [RelayCommand]
    private async Task LoadSubscriptionsAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            var data = await _expenseService.GetSubscriptionsAsync();
            Subscriptions = new ObservableCollection<Subscription>(data.OrderBy(s => s.Name));
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
    private async Task AddSubscriptionAsync()
    {
        await Shell.Current.GoToAsync("add-subscription");
    }

    [RelayCommand]
    private async Task EditSubscriptionAsync(Subscription subscription)
    {
        if (subscription == null) return;

        var parameters = new Dictionary<string, object>
        {
            ["subscription"] = subscription
        };
        await Shell.Current.GoToAsync("edit-subscription", parameters);
    }

    [RelayCommand]
    private async Task DeleteSubscriptionAsync(Subscription subscription)
    {
        if (subscription == null) return;

        var result = await Shell.Current.DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete the subscription '{subscription.Name}'?",
            "Yes", "No");

        if (result)
        {
            try
            {
                await _expenseService.DeleteSubscriptionAsync(subscription.Id);
                Subscriptions.Remove(subscription);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to delete subscription: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task ToggleSubscriptionStatusAsync(Subscription subscription)
    {
        if (subscription == null) return;

        try
        {
            subscription.IsActive = !subscription.IsActive;
            subscription.UpdatedAt = DateTime.UtcNow;

            await _expenseService.UpdateSubscriptionAsync(subscription);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to update subscription: {ex.Message}";
            // Revert the change
            subscription.IsActive = !subscription.IsActive;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadSubscriptionsAsync();
    }
}
