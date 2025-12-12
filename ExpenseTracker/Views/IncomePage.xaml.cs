using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class IncomePage : ContentPage
{
	public IncomePage(IncomesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is IncomesViewModel viewModel)
		{
			await viewModel.LoadIncomesCommand.ExecuteAsync(null);
		}
	}
}
