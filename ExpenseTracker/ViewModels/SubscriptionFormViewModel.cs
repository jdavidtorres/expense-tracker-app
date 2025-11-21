using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

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

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            // Basic validation
            if (string.IsNullOrWhiteSpace(Subscription.Name))
            {
                ErrorMessage = "Subscription name is required.";
                return;
            }

            if (Subscription.Amount <= 0)
            {
                ErrorMessage = "Amount must be greater than 0.";
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
}
