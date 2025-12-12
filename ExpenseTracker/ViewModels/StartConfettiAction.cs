using ExpenseTracker.Views.Controls;

namespace ExpenseTracker.ViewModels;

public class StartConfettiAction : TriggerAction<ConfettiView>
{
    public string TargetName { get; set; } = string.Empty;

    protected override async void Invoke(ConfettiView sender)
    {
        await sender.StartAsync();
    }
}
