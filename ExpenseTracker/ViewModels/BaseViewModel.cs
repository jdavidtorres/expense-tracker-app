using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpenseTracker.ViewModels;

/// <summary>
/// Base class for all ViewModels providing common properties and functionality
/// </summary>
public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? errorMessage;

    /// <summary>
    /// Gets a value indicating whether the ViewModel is not loading
    /// </summary>
    public bool IsNotLoading => !IsLoading;
    
    /// <summary>
    /// Gets a value indicating whether there is an error message
    /// </summary>
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// Clears the current error message
    /// </summary>
    protected void ClearError() => ErrorMessage = null;

    /// <summary>
    /// Sets an error message
    /// </summary>
    /// <param name="message">The error message to set</param>
    protected void SetError(string message) => ErrorMessage = message;

    /// <summary>
    /// Executes an async action with automatic loading state management and error handling
    /// </summary>
    /// <param name="action">The action to execute</param>
    /// <param name="errorMessage">Optional custom error message</param>
    protected async Task ExecuteAsync(Func<Task> action, string? errorMessage = null)
    {
        try
        {
            IsLoading = true;
            ClearError();
            await action().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ErrorMessage = errorMessage ?? $"An error occurred: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Executes an async function with automatic loading state management and error handling
    /// </summary>
    /// <typeparam name="T">The return type</typeparam>
    /// <param name="func">The function to execute</param>
    /// <param name="errorMessage">Optional custom error message</param>
    /// <returns>The result of the function or default value on error</returns>
    protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> func, string? errorMessage = null)
    {
        try
        {
            IsLoading = true;
            ClearError();
            return await func().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ErrorMessage = errorMessage ?? $"An error occurred: {ex.Message}";
            return default;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
