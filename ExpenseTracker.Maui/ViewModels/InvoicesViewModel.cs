using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Shared.Models;
using ExpenseTracker.Shared.Services;

namespace ExpenseTracker.Maui.ViewModels;

public partial class InvoicesViewModel : BaseViewModel
{
    private readonly ExpenseService _expenseService;

    public InvoicesViewModel(ExpenseService expenseService)
    {
        _expenseService = expenseService;
        Title = "Invoices";
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