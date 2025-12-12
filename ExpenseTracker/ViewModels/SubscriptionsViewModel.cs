using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for managing subscriptions display and operations
/// </summary>
public partial class SubscriptionsViewModel : BaseViewModel
{
    private readonly LocalExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<Subscription> subscriptions = new();

    [ObservableProperty]
    private Subscription? selectedSubscription;

    public SubscriptionsViewModel(LocalExpenseService expenseService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
    }

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

    [RelayCommand]
    private async Task DeleteSubscriptionAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

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
    private async Task NavigateToAddSubscriptionAsync()
    {
        await Shell.Current.GoToAsync("add-subscription");
    }

    [RelayCommand]
    private async Task NavigateToEditSubscriptionAsync(Subscription subscription)
    {
        if (subscription == null)
            return;

        await Shell.Current.GoToAsync($"edit-subscription?id={subscription.Id}");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadSubscriptionsAsync();
    }
}
