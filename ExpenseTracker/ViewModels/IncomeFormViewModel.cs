using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class IncomeFormViewModel : BaseViewModel
{
    private readonly LocalExpenseService _expenseService;

    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private Income income = new();

    public IncomeFormViewModel(LocalExpenseService expenseService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
    }

    [RelayCommand]
    private async Task LoadIncomeAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            Income = new Income { Date = DateTime.Now };
            return;
        }

        try
        {
            IsLoading = true;
            Income = await _expenseService.GetIncomeAsync(Id);
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
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Income.Source))
        {
            await Shell.Current.DisplayAlert("Error", "Source is required", "OK");
            return;
        }

        try
        {
            IsLoading = true;
            await _expenseService.SaveIncomeAsync(Income);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to save income: {ex.Message}";
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

    partial void OnIdChanged(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            _ = LoadIncomeAsync();
    }
}
