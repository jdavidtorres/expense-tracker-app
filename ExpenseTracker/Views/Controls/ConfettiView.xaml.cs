using Microsoft.Maui.Controls.Shapes;

namespace ExpenseTracker.Views.Controls;

public partial class ConfettiView : ContentView
{
    private readonly Random _random = new();
    private bool _isPlaying;

    public ConfettiView()
    {
        InitializeComponent();
    }

    public async Task StartAsync()
    {
        if (_isPlaying) return;
        _isPlaying = true;

        // Create 50 particles
        for (int i = 0; i < 50; i++)
        {
            CreateParticle();
        }

        // Auto-stop after 3 seconds
        await Task.Delay(3000);
        Stop();
    }

    public void Stop()
    {
        _isPlaying = false;
        ConfettiLayout.Children.Clear();
    }

    private async void CreateParticle()
    {
        if (!_isPlaying) return;

        var size = _random.Next(5, 12);
        var box = new BoxView
        {
            Color = GetRandomColor(),
            WidthRequest = size,
            HeightRequest = size,
            CornerRadius = size / 2,
            InputTransparent = true
        };

        var startX = _random.NextDouble() * (Window?.Width ?? 300);
        var startY = -20.0;

        AbsoluteLayout.SetLayoutBounds(box, new Rect(startX, startY, size, size));
        ConfettiLayout.Children.Add(box);

        // Animate
        var duration = _random.Next(1500, 3000);
        var endY = (Window?.Height ?? 800) + 20;
        var endX = startX + _random.Next(-100, 100);

        var rotation = _random.Next(360, 1080);

        var tasks = new List<Task>
        {
            box.TranslateTo(endX - startX, endY - startY, (uint)duration, Easing.CubicIn),
            box.RotateTo(rotation, (uint)duration),
            box.FadeTo(0, (uint)duration)
        };

        await Task.WhenAll(tasks);

        ConfettiLayout.Children.Remove(box);
    }

    private Color GetRandomColor()
    {
        var colors = new[]
        {
            Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow, 
            Colors.Purple, Colors.Orange, Colors.Pink, Colors.Cyan
        };
        return colors[_random.Next(colors.Length)];
    }
}
