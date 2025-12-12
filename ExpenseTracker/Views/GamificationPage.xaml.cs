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
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            await viewModel.LoadGamificationDataCommand.ExecuteAsync(null);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is GamificationViewModel viewModel)
        {
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GamificationViewModel.ShowAchievementUnlocked))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (BindingContext is GamificationViewModel vm && vm.ShowAchievementUnlocked)
                {
                    TriggerConfetti();
                }
            });
        }
    }

    private async void TriggerConfetti()
    {
        // Safety check for layout
        if (Width <= 0 || Height <= 0) return;

        // Clear previous confetti
        ConfettiContainer.Children.Clear();
        ConfettiContainer.IsVisible = true;

        var random = new Random();
        var colors = new[] { Colors.Gold, Colors.Purple, Colors.Pink, Colors.Cyan, Colors.LimeGreen };
        var particles = new List<BoxView>();

        // Create 50 particles
        for (int i = 0; i < 50; i++)
        {
            var box = new BoxView
            {
                Color = colors[random.Next(colors.Length)],
                CornerRadius = 2,
                WidthRequest = 8,
                HeightRequest = 8,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                TranslationX = ConfettiContainer.Width / 2, // Start middle
                TranslationY = -20 // Start just above
            };

            ConfettiContainer.Children.Add(box);
            particles.Add(box);
        }

        // Animate them
        var tasks = new List<Task>();
        foreach (var particle in particles)
        {
            // Randomize trajectory
            double endX = random.NextDouble() * Width;
            double endY = Height + 50;
            uint duration = (uint)random.Next(1500, 3000);
            
            // Initial random spread
            particle.TranslationX = random.NextDouble() * Width;
            particle.TranslationY = -random.Next(0, 100);

            var t1 = particle.TranslateTo(particle.TranslationX, endY, duration, Easing.BounceOut);
            var t2 = particle.RotateTo(random.Next(180, 720), duration);
            var t3 = particle.FadeTo(0, duration);

            tasks.Add(Task.WhenAll(t1, t2, t3));
        }

        await Task.WhenAll(tasks);
        
        // Cleanup
        ConfettiContainer.Children.Clear();
        ConfettiContainer.IsVisible = false;
    }
}
