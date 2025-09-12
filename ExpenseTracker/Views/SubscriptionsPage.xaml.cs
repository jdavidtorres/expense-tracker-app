using ExpenseTracker.Maui.ViewModels;

namespace ExpenseTracker.Views;

public partial class SubscriptionsPage : ContentPage
{
    private readonly SubscriptionsViewModel _viewModel;

    public SubscriptionsPage(SubscriptionsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSubscriptionsCommand.ExecuteAsync(null);
    }
}