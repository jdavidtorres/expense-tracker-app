<<<<<<< HEAD
﻿using Microsoft.Extensions.Logging;
using ExpenseTracker.Services;
using ExpenseTracker.ViewModels;
using ExpenseTracker.Views;

namespace ExpenseTracker
{
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
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			// Configure HttpClient for API calls
			builder.Services.AddHttpClient<ExpenseService>(client =>
			{
				client.BaseAddress = new Uri("http://localhost:8083/api/");
				client.Timeout = TimeSpan.FromSeconds(30);
			});

			// Register pages
			builder.Services.AddTransient<DashboardPage>();
			builder.Services.AddTransient<SubscriptionsPage>();
			builder.Services.AddTransient<InvoicesPage>();
			builder.Services.AddTransient<SubscriptionFormPage>();
			builder.Services.AddTransient<InvoiceFormPage>();

			// Register ViewModels
			builder.Services.AddTransient<DashboardViewModel>();
			builder.Services.AddTransient<SubscriptionsViewModel>();
			builder.Services.AddTransient<InvoicesViewModel>();
			builder.Services.AddTransient<SubscriptionFormViewModel>();
			builder.Services.AddTransient<InvoiceFormViewModel>();

			// Register services
			builder.Services.AddTransient<ExpenseService>();

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
=======
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
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
	}
}
