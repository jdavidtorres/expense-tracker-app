using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

namespace ExpenseTracker.ViewModels;

public partial class SavingsGoalsViewModel : BaseViewModel
{
    private readonly LocalExpenseService _expenseService;

    [ObservableProperty]
    private ObservableCollection<SavingsGoal> savingsGoals = new();

    public SavingsGoalsViewModel(LocalExpenseService expenseService)
    {
        _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
    }

    [RelayCommand]
    private async Task LoadSavingsGoalsAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();
            var data = await _expenseService.GetSavingsGoalsAsync();
            SavingsGoals = new ObservableCollection<SavingsGoal>(data);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load savings goals: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteSavingsGoalAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        try
        {
            await _expenseService.DeleteSavingsGoalAsync(id);
            await LoadSavingsGoalsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete savings goal: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task NavigateToAddSavingsGoalAsync()
    {
        await Shell.Current.GoToAsync("savings-form");
    }

    [RelayCommand]
    private async Task NavigateToEditSavingsGoalAsync(SavingsGoal goal)
    {
        if (goal == null) return;
        await Shell.Current.GoToAsync($"savings-form?id={goal.Id}");
    }
}
