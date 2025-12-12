using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class SavingsGoalFormViewModel : BaseViewModel
{
    private readonly LocalExpenseService _expenseService;

    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private SavingsGoal goal = new();

    public SavingsGoalFormViewModel(LocalExpenseService expenseService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
    }

    [RelayCommand]
    private async Task LoadGoalAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            Goal = new SavingsGoal { Deadline = DateTime.Now.AddMonths(6) };
            return;
        }

        try
        {
            IsLoading = true;
            Goal = await _expenseService.GetSavingsGoalAsync(Id);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load goal: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Goal.Name))
        {
            await Shell.Current.DisplayAlert("Error", "Name is required", "OK");
            return;
        }

        try
        {
            IsLoading = true;
            await _expenseService.SaveSavingsGoalAsync(Goal);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to save goal: {ex.Message}";
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
            _ = LoadGoalAsync();
    }
}
