using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for managing subscriptions
/// </summary>
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

    /// <summary>
    /// Loads all subscriptions from the service
    /// </summary>
    [RelayCommand]
    private async Task LoadSubscriptionsAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

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

    /// <summary>
    /// Deletes a subscription
    /// </summary>
    [RelayCommand]
    private async Task DeleteSubscriptionAsync(Subscription subscription)
    {
        ArgumentNullException.ThrowIfNull(subscription);

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

    /// <summary>
    /// Navigates to add a new subscription
    /// </summary>
    [RelayCommand]
    private async Task AddSubscriptionAsync()
    {
        await Shell.Current.GoToAsync("add-subscription");
    }

    /// <summary>
    /// Navigates to edit an existing subscription
    /// </summary>
    [RelayCommand]
    private async Task EditSubscriptionAsync(Subscription subscription)
    {
        ArgumentNullException.ThrowIfNull(subscription);
        await Shell.Current.GoToAsync($"edit-subscription?id={subscription.Id}");
    }

    /// <summary>
    /// Refreshes the subscriptions list
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadSubscriptionsAsync();
    }

    /// <summary>
    /// Toggles subscription status (active/inactive)
    /// </summary>
    [RelayCommand]
    private async Task ToggleSubscriptionStatusAsync(Subscription subscription)
    {
        ArgumentNullException.ThrowIfNull(subscription);

        try
        {
            subscription.IsActive = !subscription.IsActive;
            subscription.UpdatedAt = DateTime.UtcNow;
            await _expenseService.UpdateSubscriptionAsync(subscription);
            await LoadSubscriptionsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to toggle subscription status: {ex.Message}";
        }
    }
}
