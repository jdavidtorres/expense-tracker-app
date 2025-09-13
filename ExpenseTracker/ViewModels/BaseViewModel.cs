using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpenseTracker.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? errorMessage;

    public bool IsNotLoading => !IsLoading;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    protected void ClearError() => ErrorMessage = null;
}