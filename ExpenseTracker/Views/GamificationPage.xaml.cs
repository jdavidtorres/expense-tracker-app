using ExpenseTracker.ViewModels;

namespace ExpenseTracker.Views;

public partial class GamificationPage : ContentPage
{
    public GamificationPage(GamificationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is GamificationViewModel viewModel)
        {
            await viewModel.LoadGamificationDataCommand.ExecuteAsync(null);
        }
    }
}
