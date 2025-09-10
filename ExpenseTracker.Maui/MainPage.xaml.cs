namespace ExpenseTracker.Maui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnDashboardClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//dashboard");
	}
}
