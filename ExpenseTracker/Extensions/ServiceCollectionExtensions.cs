using ExpenseTracker.Data;
using ExpenseTracker.Services;
using ExpenseTracker.ViewModels;
using ExpenseTracker.Views;

namespace ExpenseTracker.Extensions;

/// <summary>
/// Extension methods for service registration
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all application services
    /// </summary>
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        // Register SQLite database service as singleton
        services.AddSingleton<DatabaseService>();

        // Register local expense service (SQLite-based, replaces HTTP ExpenseService)
        services.AddSingleton<LocalExpenseService>();

        // Register singleton services
        services.AddSingleton<GamificationService>();

        return services;
    }

    /// <summary>
    /// Registers all ViewModels
    /// </summary>
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<GamificationViewModel>();
        services.AddTransient<SubscriptionsViewModel>();
        services.AddTransient<InvoicesViewModel>();
        services.AddTransient<SubscriptionFormViewModel>();
        services.AddTransient<InvoiceFormViewModel>();

        services.AddTransient<DebtsViewModel>();
        services.AddTransient<DebtFormViewModel>();
        services.AddTransient<IncomesViewModel>();
        services.AddTransient<IncomeFormViewModel>();
        services.AddTransient<SavingsGoalsViewModel>();
        services.AddTransient<SavingsGoalFormViewModel>();

        return services;
    }

    /// <summary>
    /// Registers all Pages/Views
    /// </summary>
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddTransient<DashboardPage>();
        services.AddTransient<GamificationPage>();
        services.AddTransient<SubscriptionsPage>();
        services.AddTransient<InvoicesPage>();
        services.AddTransient<SubscriptionFormPage>();
        services.AddTransient<InvoiceFormPage>();

        services.AddTransient<DebtsPage>();
        services.AddTransient<DebtFormPage>();
        services.AddTransient<IncomePage>();
        services.AddTransient<IncomeFormPage>();
        services.AddTransient<SavingsGoalsPage>();
        services.AddTransient<SavingsGoalFormPage>();

        return services;
    }
}
