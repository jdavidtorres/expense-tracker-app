<<<<<<< HEAD
﻿using Foundation;

namespace ExpenseTracker
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}
=======
using Foundation;

namespace ExpenseTracker.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
