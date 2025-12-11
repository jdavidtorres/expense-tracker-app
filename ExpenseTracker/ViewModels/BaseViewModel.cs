using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// Base class for all ViewModels providing common properties and functionality
/// </summary>
public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    private bool isLoading;

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? errorMessage;

    /// <summary>
    /// Gets whether the ViewModel is not currently loading (inverse of IsLoading)
    /// </summary>
    public bool IsNotLoading => !IsLoading;

    /// <summary>
    /// Gets whether the ViewModel has an error message
    /// </summary>
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// Clears the current error message
    /// </summary>
    protected void ClearError() => ErrorMessage = null;
}