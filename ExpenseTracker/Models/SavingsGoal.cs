namespace ExpenseTracker.Models;

public class SavingsGoal
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime? Deadline { get; set; }
    public string Icon { get; set; } = "piggy_bank";
    public string Notes { get; set; } = string.Empty;
    public double Progress => TargetAmount > 0 ? (double)(CurrentAmount / TargetAmount) : 0;
}
