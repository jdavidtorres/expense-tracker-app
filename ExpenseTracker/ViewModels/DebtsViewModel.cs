using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class DebtsViewModel : BaseViewModel
{
    private readonly LocalExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<Debt> debts = new();

    public DebtsViewModel(LocalExpenseService expenseService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
    }

    [RelayCommand]
    private async Task LoadDebtsAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();
            var data = await _expenseService.GetDebtsAsync();
            Debts = new ObservableCollection<Debt>(data);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load debts: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteDebtAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        try
        {
            await _expenseService.DeleteDebtAsync(id);
            await LoadDebtsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete debt: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task NavigateToAddDebtAsync()
    {
        await Shell.Current.GoToAsync("debt-form");
    }

    [RelayCommand]
    private async Task NavigateToEditDebtAsync(Debt debt)
    {
        if (debt == null) return;
        await Shell.Current.GoToAsync($"debt-form?id={debt.Id}");
    }
}
