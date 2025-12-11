using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for invoice form (add/edit)
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
    /// Gets the list of available invoice statuses
    /// </summary>
    public List<InvoiceStatus> InvoiceStatuses { get; } = Enum.GetValues<InvoiceStatus>().ToList();

    public InvoiceFormViewModel(ExpenseService expenseService, GamificationService gamificationService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
        _gamificationService = gamificationService ?? throw new ArgumentNullException(nameof(gamificationService));
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

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!ValidateInvoice())
            return;

        try
        {
            IsLoading = true;
            ClearError();

            Invoice.UpdatedAt = DateTime.UtcNow;

            if (IsEditing)
            {
                await _expenseService.UpdateInvoiceAsync(Invoice).ConfigureAwait(false);
            }
            else
            {
                Invoice.CreatedAt = DateTime.UtcNow;
                await _expenseService.CreateInvoiceAsync(Invoice).ConfigureAwait(false);
                
                // Award points for adding a new invoice
                var newAchievements = await _gamificationService.RecordExpenseTrackedAsync().ConfigureAwait(false);
                
                // Show achievement notification if any were unlocked
                if (newAchievements.Any())
                {
                    var achievement = newAchievements.First();
                    await Application.Current!.MainPage!.DisplayAlert(
                        "🎉 Achievement Unlocked!",
                        $"{achievement.Icon} {achievement.Name}\n{achievement.Description}\n+{achievement.PointsReward} Points!",
                        "Awesome!").ConfigureAwait(false);
                }
            }

            await Shell.Current.GoToAsync("..").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to save invoice: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..").ConfigureAwait(false);
    }

    /// <summary>
    /// Validates the invoice data
    /// </summary>
    /// <returns>True if valid, false otherwise</returns>
    private bool ValidateInvoice()
    {
        if (string.IsNullOrWhiteSpace(Invoice.InvoiceNumber))
        {
            SetError("Invoice number is required.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(Invoice.Name))
        {
            SetError("Invoice name is required.");
            return false;
        }

        if (Invoice.Amount <= 0)
        {
            SetError("Amount must be greater than 0.");
            return false;
        }

        return true;
    }
}
