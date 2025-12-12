using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class IncomesViewModel : BaseViewModel
{
    private readonly LocalExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<Income> incomes = new();

    public IncomesViewModel(LocalExpenseService expenseService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
    }

    [RelayCommand]
    private async Task LoadIncomesAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();
            var data = await _expenseService.GetIncomesAsync();
            Incomes = new ObservableCollection<Income>(data);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load income: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteIncomeAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        try
        {
            await _expenseService.DeleteIncomeAsync(id);
            await LoadIncomesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete income: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task NavigateToAddIncomeAsync()
    {
        await Shell.Current.GoToAsync("income-form");
    }

    [RelayCommand]
    private async Task NavigateToEditIncomeAsync(Income income)
    {
        if (income == null) return;
        await Shell.Current.GoToAsync($"income-form?id={income.Id}");
    }
}
