<<<<<<< HEAD
﻿using Android.App;
using Android.Runtime;

namespace ExpenseTracker
{
	[Application]
	public class MainApplication : MauiApplication
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership)
			: base(handle, ownership)
		{
		}

		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}
=======
using Android.App;
using Android.Runtime;

namespace ExpenseTracker.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
