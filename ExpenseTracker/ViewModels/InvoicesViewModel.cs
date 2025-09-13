using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class InvoicesViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<Invoice> invoices = new();

    [ObservableProperty]
    private Invoice? selectedInvoice;

    [ObservableProperty]
    private InvoiceStatus filterStatus = InvoiceStatus.Pending;

    [ObservableProperty]
    private bool showAllStatuses = true;

    public InvoicesViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [RelayCommand]
    private async Task LoadInvoicesAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var allInvoices = await _expenseService.GetInvoicesAsync();
            
            if (ShowAllStatuses)
            {
                Invoices = new ObservableCollection<Invoice>(allInvoices);
            }
            else
            {
                var filteredInvoices = allInvoices.Where(i => i.Status == FilterStatus);
                Invoices = new ObservableCollection<Invoice>(filteredInvoices);
            }
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
    private async Task NavigateToAddInvoiceAsync()
    {
        await Shell.Current.GoToAsync("add-invoice");
    }

    [RelayCommand]
    private async Task NavigateToEditInvoiceAsync(Invoice invoice)
    {
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
