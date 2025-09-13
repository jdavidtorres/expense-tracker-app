using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class InvoicesViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

<<<<<<< HEAD
    [ObservableProperty]
    private ObservableCollection<Invoice> invoices = new();

    [ObservableProperty]
    private Invoice? selectedInvoice;

    [ObservableProperty]
    private InvoiceStatus filterStatus = InvoiceStatus.Pending;

    [ObservableProperty]
    private bool showAllStatuses = true;

=======
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
    public InvoicesViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
        Title = "Invoices";
<<<<<<< HEAD
    }

    [RelayCommand]
    private async Task LoadInvoicesAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            var data = await _expenseService.GetInvoicesAsync();
            var filteredData = ShowAllStatuses
                ? data
                : data.Where(i => i.Status == FilterStatus);

            Invoices = new ObservableCollection<Invoice>(
                filteredData.OrderByDescending(i => i.IssueDate));
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
    private async Task AddInvoiceAsync()
    {
        await Shell.Current.GoToAsync("add-invoice");
    }

    [RelayCommand]
    private async Task EditInvoiceAsync(Invoice invoice)
    {
        if (invoice == null) return;

        var parameters = new Dictionary<string, object>
        {
            ["invoice"] = invoice
        };
        await Shell.Current.GoToAsync("edit-invoice", parameters);
    }

    [RelayCommand]
    private async Task DeleteInvoiceAsync(Invoice invoice)
    {
        if (invoice == null) return;

        var result = await Shell.Current.DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete invoice '{invoice.InvoiceNumber}'?",
            "Yes", "No");

        if (result)
        {
            try
            {
                await _expenseService.DeleteInvoiceAsync(invoice.Id);
                Invoices.Remove(invoice);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to delete invoice: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task MarkAsPaidAsync(Invoice invoice)
    {
        if (invoice == null) return;

        try
        {
            invoice.Status = InvoiceStatus.Paid;
            invoice.UpdatedAt = DateTime.UtcNow;

            await _expenseService.UpdateInvoiceAsync(invoice);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to update invoice: {ex.Message}";
            // Revert the change
            invoice.Status = InvoiceStatus.Pending;
        }
    }

    [RelayCommand]
    private async Task FilterByStatusAsync(InvoiceStatus status)
    {
        FilterStatus = status;
        ShowAllStatuses = false;
        await LoadInvoicesAsync();
    }

    [RelayCommand]
    private async Task ShowAllAsync()
    {
        ShowAllStatuses = true;
        await LoadInvoicesAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadInvoicesAsync();
    }
}
=======
        Invoices = new ObservableCollection<Invoice>();
    }

    [ObservableProperty]
    private ObservableCollection<Invoice> invoices;

    [RelayCommand]
    private async Task LoadInvoicesAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            var invoices = await _expenseService.GetInvoicesAsync();
            
            Invoices.Clear();
            foreach (var invoice in invoices)
            {
                Invoices.Add(invoice);
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load invoices: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
