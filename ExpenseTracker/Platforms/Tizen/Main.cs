<<<<<<< HEAD
using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ExpenseTracker
{
	internal class Program : MauiApplication
	{
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

		static void Main(string[] args)
		{
			var app = new Program();
			app.Run(args);
		}
	}
}
=======
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ExpenseTracker.Platforms.Tizen;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
