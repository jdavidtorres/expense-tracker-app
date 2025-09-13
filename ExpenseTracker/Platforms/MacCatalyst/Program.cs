<<<<<<< HEAD
﻿using ObjCRuntime;
using UIKit;

namespace ExpenseTracker
{
	public class Program
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, typeof(AppDelegate));
		}
	}
}
=======
using UIKit;

namespace ExpenseTracker.Platforms.MacCatalyst;

public class Program
{
	// This is the main entry point of the application.
	static void Main(string[] args)
	{
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
