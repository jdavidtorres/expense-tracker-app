using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class SubscriptionsPage : ContentPage
{
<<<<<<< HEAD
    public SubscriptionsPage(SubscriptionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
=======
    private readonly SubscriptionsViewModel _viewModel;

    public SubscriptionsPage(SubscriptionsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
<<<<<<< HEAD

        if (BindingContext is SubscriptionsViewModel viewModel)
        {
            await viewModel.LoadSubscriptionsCommand.ExecuteAsync(null);
        }
    }
}
=======
        await _viewModel.LoadSubscriptionsCommand.ExecuteAsync(null);
    }
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
