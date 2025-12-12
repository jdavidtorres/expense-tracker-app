namespace ExpenseTracker.Models;

public class Income
{
    public string Id { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsRecurring { get; set; }
    public string Notes { get; set; } = string.Empty;
}
