using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class SubscriptionsPage : ContentPage
{
    public SubscriptionsPage(SubscriptionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SubscriptionsViewModel viewModel)
        {
            await viewModel.LoadSubscriptionsCommand.ExecuteAsync(null);
        }
    }
}