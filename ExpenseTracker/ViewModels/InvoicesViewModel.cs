using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for managing invoices display and operations
/// </summary>
public partial class InvoicesViewModel : BaseViewModel
{
    private readonly LocalExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<Invoice> invoices = new();

    [ObservableProperty]
    private Invoice? selectedInvoice;

    [ObservableProperty]
    private InvoiceStatus filterStatus = InvoiceStatus.Pending;

    [ObservableProperty]
    private bool showAllStatuses = true;

    public InvoicesViewModel(LocalExpenseService expenseService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
    }

    [RelayCommand]
    private async Task LoadInvoicesAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            var allInvoices = await _expenseService.GetInvoicesAsync();

            Invoices = new ObservableCollection<Invoice>(
                ShowAllStatuses
                    ? allInvoices
                    : allInvoices.Where(i => i.Status == FilterStatus)
            );
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load invoices: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteInvoiceAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        try
        {
            await _expenseService.DeleteInvoiceAsync(id);
            await LoadInvoicesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete invoice: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task MarkAsPaidAsync(Invoice invoice)
    {
        if (invoice == null)
            return;

        try
        {
            // Update invoice status to paid
            invoice.Status = InvoiceStatus.Paid;
            await _expenseService.UpdateInvoiceAsync(invoice);
            await LoadInvoicesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to mark invoice as paid: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task NavigateToAddInvoiceAsync()
    {
        await Shell.Current.GoToAsync("add-invoice");
    }

    [RelayCommand]
    private async Task NavigateToEditInvoiceAsync(Invoice invoice)
    {
        if (invoice == null)
            return;

        await Shell.Current.GoToAsync($"edit-invoice?id={invoice.Id}");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadInvoicesAsync();
    }

    partial void OnFilterStatusChanged(InvoiceStatus value)
    {
        Task.Run(LoadInvoicesAsync);
    }

    partial void OnShowAllStatusesChanged(bool value)
    {
        Task.Run(LoadInvoicesAsync);
    }
}
