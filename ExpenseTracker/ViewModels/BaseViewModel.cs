using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpenseTracker.ViewModels;

<<<<<<< HEAD
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
=======
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool hasError;

    protected void SetError(string message)
    {
        ErrorMessage = message;
        HasError = !string.IsNullOrEmpty(message);
    }

    protected void ClearError()
    {
        ErrorMessage = string.Empty;
        HasError = false;
    }
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
