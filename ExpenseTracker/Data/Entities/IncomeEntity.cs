using SQLite;

namespace ExpenseTracker.Data.Entities;

[Table("income")]
public class IncomeEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Source { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string Category { get; set; } = "Salary";

    public bool IsRecurring { get; set; }

    public string Notes { get; set; } = string.Empty;
}
