using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for subscription form (add/edit)
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
    /// Gets the list of available billing cycles
    /// </summary>
    public List<BillingCycle> BillingCycles { get; } = Enum.GetValues<BillingCycle>().ToList();

    public SubscriptionFormViewModel(ExpenseService expenseService, GamificationService gamificationService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
        _gamificationService = gamificationService ?? throw new ArgumentNullException(nameof(gamificationService));
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

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!ValidateSubscription())
            return;

        try
        {
            IsLoading = true;
            ClearError();

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
                if (newAchievements.Any())
                {
                    var achievement = newAchievements.First();
                    await Application.Current!.MainPage!.DisplayAlert(
                        "🎉 Achievement Unlocked!",
                        $"{achievement.Icon} {achievement.Name}\n{achievement.Description}\n+{achievement.PointsReward} Points!",
                        "Awesome!");
                }
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

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Validates the subscription data
    /// </summary>
    /// <returns>True if valid, false otherwise</returns>
    private bool ValidateSubscription()
    {
        if (string.IsNullOrWhiteSpace(Subscription.Name))
        {
            SetError("Subscription name is required.");
            return false;
        }

        if (Subscription.Amount <= 0)
        {
            SetError("Amount must be greater than 0.");
            return false;
        }

        return true;
    }
}
