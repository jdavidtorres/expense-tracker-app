using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

[QueryProperty(nameof(Invoice), "invoice")]
public partial class InvoiceFormViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

    [ObservableProperty]
    private Invoice invoice = new();

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private string formTitle = "Add Invoice";

    public List<InvoiceStatus> InvoiceStatuses { get; } = Enum.GetValues<InvoiceStatus>().ToList();

    public InvoiceFormViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
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
        try
        {
            IsLoading = true;
            ClearError();

            // Basic validation
            if (string.IsNullOrWhiteSpace(Invoice.InvoiceNumber))
            {
                ErrorMessage = "Invoice number is required.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Invoice.Name))
            {
                ErrorMessage = "Invoice name is required.";
                return;
            }

            if (Invoice.Amount <= 0)
            {
                ErrorMessage = "Amount must be greater than 0.";
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
            }

            await Shell.Current.GoToAsync("..");
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
        await Shell.Current.GoToAsync("..");
    }
}
