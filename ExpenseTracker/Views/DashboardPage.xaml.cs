using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class DashboardPage : ContentPage
{
<<<<<<< HEAD
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
=======
    private readonly DashboardViewModel _viewModel;

    public DashboardPage(DashboardViewModel viewModel)
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

        if (BindingContext is DashboardViewModel viewModel)
        {
            await viewModel.LoadDashboardDataCommand.ExecuteAsync(null);
        }
    }
}
=======
        await _viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
