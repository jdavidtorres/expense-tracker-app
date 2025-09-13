<<<<<<< HEAD
﻿namespace ExpenseTracker
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(new AppShell());
		}
	}
}
=======
namespace ExpenseTracker;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
