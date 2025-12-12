using SQLite;

namespace ExpenseTracker.Data.Entities;

[Table("debts")]
public class DebtEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Source { get; set; } = string.Empty; // Who owes / is owed

    public decimal Amount { get; set; }

    public DateTime? DueDate { get; set; }

    public string Status { get; set; } = "Pending"; // Pending, Paid

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public bool IsOwedToMe { get; set; } // true = I lent money, false = I borrowed
}
