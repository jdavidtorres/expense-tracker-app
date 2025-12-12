using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class SavingsGoalsPage : ContentPage
{
	public SavingsGoalsPage(SavingsGoalsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is SavingsGoalsViewModel viewModel)
		{
			await viewModel.LoadSavingsGoalsCommand.ExecuteAsync(null);
		}
	}
}
