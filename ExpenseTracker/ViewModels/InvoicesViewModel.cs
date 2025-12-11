using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// ViewModel for managing invoices
/// </summary>
public partial class InvoicesViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<Invoice> invoices = new();

    [ObservableProperty]
    private Invoice? selectedInvoice;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredInvoices))]
    private InvoiceStatus filterStatus = InvoiceStatus.Pending;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredInvoices))]
    private bool showAllStatuses = true;

    /// <summary>
    /// Gets filtered invoices based on current filter settings
    /// </summary>
    public ObservableCollection<Invoice> FilteredInvoices
    {
        get
        {
            if (ShowAllStatuses)
            {
                return Invoices;
            }

            var filtered = Invoices.Where(i => i.Status == FilterStatus).ToList();
            return new ObservableCollection<Invoice>(filtered);
        }
    }

    public InvoicesViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Loads all invoices from the service
    /// </summary>
    [RelayCommand]
    private async Task LoadInvoicesAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            var allInvoices = await _expenseService.GetInvoicesAsync();
            Invoices = new ObservableCollection<Invoice>(allInvoices);
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

    /// <summary>
    /// Deletes an invoice
    /// </summary>
    [RelayCommand]
    private async Task DeleteInvoiceAsync(Invoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);

        try
        {
            await _expenseService.DeleteInvoiceAsync(invoice.Id);
            await LoadInvoicesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete invoice: {ex.Message}";
        }
    }

    /// <summary>
    /// Marks an invoice as paid
    /// </summary>
    [RelayCommand]
    private async Task MarkAsPaidAsync(Invoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);

        try
        {
            invoice.Status = InvoiceStatus.Paid;
            invoice.UpdatedAt = DateTime.UtcNow;
            await _expenseService.UpdateInvoiceAsync(invoice);
            await LoadInvoicesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to mark invoice as paid: {ex.Message}";
        }
    }

    /// <summary>
    /// Navigates to edit an invoice
    /// </summary>
    [RelayCommand]
    private async Task EditInvoiceAsync(Invoice invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        await Shell.Current.GoToAsync($"edit-invoice?id={invoice.Id}");
    }

    /// <summary>
    /// Navigates to add a new invoice
    /// </summary>
    [RelayCommand]
    private async Task AddInvoiceAsync()
    {
        await Shell.Current.GoToAsync("add-invoice");
    }

    /// <summary>
    /// Refreshes the invoices list
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadInvoicesAsync();
    }
}
