<<<<<<< HEAD
﻿namespace ExpenseTracker
{
	public partial class MainPage : ContentPage
	{
		int count = 0;

		public MainPage()
		{
			InitializeComponent();
		}

		private void OnCounterClicked(object? sender, EventArgs e)
		{
			count++;

			if (count == 1)
				CounterBtn.Text = $"Clicked {count} time";
			else
				CounterBtn.Text = $"Clicked {count} times";

			SemanticScreenReader.Announce(CounterBtn.Text);
		}
=======
namespace ExpenseTracker;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnDashboardClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//dashboard");
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
	}
}
