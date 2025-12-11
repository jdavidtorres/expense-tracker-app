using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for adding or editing a subscription
/// </summary>
[QueryProperty(nameof(Subscription), "subscription")]
public partial class SubscriptionFormViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;
    private readonly GamificationService _gamificationService;

    [ObservableProperty]
    private Subscription subscription = new();

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private string formTitle = "Add Subscription";

    /// <summary>
    /// List of available billing cycles for selection
    /// </summary>
    public List<BillingCycle> BillingCycles { get; } = Enum.GetValues<BillingCycle>().ToList();

    public SubscriptionFormViewModel(ExpenseService expenseService, GamificationService gamificationService)
    {
        _expenseService = expenseService;
        _gamificationService = gamificationService;
    }

    partial void OnSubscriptionChanged(Subscription value)
    {
        if (value?.Id != null && !string.IsNullOrEmpty(value.Id))
        {
            IsEditing = true;
            FormTitle = "Edit Subscription";
        }
        else
        {
            IsEditing = false;
            FormTitle = "Add Subscription";
        }
    }

    /// <summary>
    /// Validates and saves the subscription
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            // Validate subscription data
            if (!ValidateSubscription(out var validationError))
            {
                ErrorMessage = validationError;
                return;
            }

            Subscription.UpdatedAt = DateTime.UtcNow;

            if (IsEditing)
            {
                await _expenseService.UpdateSubscriptionAsync(Subscription);
            }
            else
            {
                Subscription.CreatedAt = DateTime.UtcNow;
                await _expenseService.CreateSubscriptionAsync(Subscription);
                
                // Award points for adding a new subscription
                var newAchievements = await _gamificationService.RecordExpenseTrackedAsync();
                
                // Show achievement notification if any were unlocked
                await ShowAchievementNotificationAsync(newAchievements);
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to save subscription: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Validates subscription data
    /// </summary>
    private bool ValidateSubscription(out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(Subscription.Name))
        {
            errorMessage = "Subscription name is required.";
            return false;
        }

        if (Subscription.Amount <= 0)
        {
            errorMessage = "Amount must be greater than 0.";
            return false;
        }

        if (Subscription.NextBillingDate < DateTime.Today)
        {
            errorMessage = "Next billing date cannot be in the past.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    /// <summary>
    /// Shows achievement notification if any achievements were unlocked
    /// </summary>
    private async Task ShowAchievementNotificationAsync(List<Achievement> newAchievements)
    {
        if (newAchievements.Any() && Application.Current?.MainPage != null)
        {
            var achievement = newAchievements.First();
            await Application.Current.MainPage.DisplayAlert(
                "🎉 Achievement Unlocked!",
                $"{achievement.Icon} {achievement.Name}\n{achievement.Description}\n+{achievement.PointsReward} Points!",
                "Awesome!");
        }
    }

    /// <summary>
    /// Cancels the form and navigates back
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
