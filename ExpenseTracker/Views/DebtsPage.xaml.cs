using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class DebtsPage : ContentPage
{
	public DebtsPage(DebtsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is DebtsViewModel viewModel)
		{
			await viewModel.LoadDebtsCommand.ExecuteAsync(null);
		}
	}
}
