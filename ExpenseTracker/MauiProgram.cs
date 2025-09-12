using ExpenseTracker.Services;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		// Configure HttpClient for API calls
		builder.Services.AddHttpClient<ExpenseService>(client =>
		{
			client.BaseAddress = new Uri("http://localhost:8083/api/");
		});

		// Register pages
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<Views.DashboardPage>();
		builder.Services.AddTransient<Views.SubscriptionsPage>();
		builder.Services.AddTransient<Views.InvoicesPage>();

		// Register ViewModels
		builder.Services.AddTransient<ViewModels.DashboardViewModel>();
		builder.Services.AddTransient<ViewModels.SubscriptionsViewModel>();
		builder.Services.AddTransient<ViewModels.InvoicesViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
