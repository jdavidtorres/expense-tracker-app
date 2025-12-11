using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Constants;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for adding or editing an invoice
/// </summary>
[QueryProperty(nameof(Invoice), "invoice")]
public partial class InvoiceFormViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;
    private readonly GamificationService _gamificationService;

    [ObservableProperty]
    private Invoice invoice = new();

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private string formTitle = "Add Invoice";

    /// <summary>
    /// List of available invoice statuses for selection
    /// </summary>
    public List<InvoiceStatus> InvoiceStatuses { get; } = Enum.GetValues<InvoiceStatus>().ToList();

    public InvoiceFormViewModel(ExpenseService expenseService, GamificationService gamificationService)
    {
        _expenseService = expenseService;
        _gamificationService = gamificationService;
    }

    partial void OnInvoiceChanged(Invoice value)
    {
        if (value?.Id != null && !string.IsNullOrEmpty(value.Id))
        {
            IsEditing = true;
            FormTitle = "Edit Invoice";
        }
        else
        {
            IsEditing = false;
            FormTitle = "Add Invoice";
        }
    }

    /// <summary>
    /// Validates and saves the invoice
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            // Validate invoice data
            if (!ValidateInvoice(out var validationError))
            {
                ErrorMessage = validationError;
                return;
            }

            Invoice.UpdatedAt = DateTime.UtcNow;

            if (IsEditing)
            {
                await _expenseService.UpdateInvoiceAsync(Invoice);
            }
            else
            {
                Invoice.CreatedAt = DateTime.UtcNow;
                await _expenseService.CreateInvoiceAsync(Invoice);
                
                // Award points for adding a new invoice
                var newAchievements = await _gamificationService.RecordExpenseTrackedAsync();
                
                // Show achievement notification if any were unlocked
                await ShowAchievementNotificationAsync(newAchievements);
            }

            await Shell.Current.GoToAsync(NavigationRoutes.NavigateBack);
        }
        catch (Exception ex)
        {
            ErrorMessage = string.Format(ErrorMessages.SaveFailed, "invoice", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Validates invoice data
    /// </summary>
    private bool ValidateInvoice(out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(Invoice.InvoiceNumber))
        {
            errorMessage = "Invoice number is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Invoice.Name))
        {
            errorMessage = ErrorMessages.NameRequired;
            return false;
        }

        if (Invoice.Amount <= 0)
        {
            errorMessage = ErrorMessages.AmountMustBeGreaterThanZero;
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
        await Shell.Current.GoToAsync(NavigationRoutes.NavigateBack);
    }
}
