using Microsoft.Extensions.Logging;
using ExpenseTracker.Extensions;

namespace ExpenseTracker
{
	/// <summary>
	/// Main program entry point for MAUI application
	/// </summary>
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

			// Register application services, ViewModels, and Views using extension methods
			builder.Services
				.AddAppServices()
				.AddViewModels()
				.AddViews();

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
