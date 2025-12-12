using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class DebtFormViewModel : BaseViewModel
{
    private readonly LocalExpenseService _expenseService;

    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private Debt debt = new();

    public DebtFormViewModel(LocalExpenseService expenseService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
    }

    [RelayCommand]
    private async Task LoadDebtAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            Debt = new Debt { DueDate = DateTime.Now.AddMonths(1) };
            return;
        }

        try
        {
            IsLoading = true;
            // Use the numeric ID directly if possible, or parse it (logic is inside this method usually or service)
            // But here we are just passing Id string to service. Service handles parsing?
            // Service expects string for GetDebtAsync? No, service expects int.
            // ViewModel GetDebtAsync helper? 
            // My Service update added GetDebtAsync(string id). 
            // Yes.
            Debt = await _expenseService.GetDebtAsync(Id);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load debt: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Debt.Source))
        {
            await Shell.Current.DisplayAlert("Error", "Source is required", "OK");
            return;
        }

        try
        {
            IsLoading = true;
            await _expenseService.SaveDebtAsync(Debt);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to save debt: {ex.Message}";
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
             _ = LoadDebtAsync();
    }
}
