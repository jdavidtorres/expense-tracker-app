using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class SubscriptionsViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

<<<<<<< HEAD
    [ObservableProperty]
    private ObservableCollection<Subscription> subscriptions = new();

    [ObservableProperty]
    private Subscription? selectedSubscription;

=======
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
    public SubscriptionsViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
        Title = "Subscriptions";
<<<<<<< HEAD
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
=======
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
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
