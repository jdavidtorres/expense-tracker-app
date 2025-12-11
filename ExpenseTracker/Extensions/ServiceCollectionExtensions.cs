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
        // Configure HttpClient for API calls
        services.AddHttpClient<ExpenseService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8083/api/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

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

        return services;
    }
}
