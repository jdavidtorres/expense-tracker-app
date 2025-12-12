namespace ExpenseTracker.Models;

public class Debt
{
    public string Id { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime? DueDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string Notes { get; set; } = string.Empty;
    public bool IsOwedToMe { get; set; }
}
