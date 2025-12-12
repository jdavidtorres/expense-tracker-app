using SQLite;

namespace ExpenseTracker.Data.Entities;

[Table("savings_goals")]
public class SavingsGoalEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;

    public decimal TargetAmount { get; set; }

    public decimal CurrentAmount { get; set; }

    public DateTime? Deadline { get; set; }

    public string Icon { get; set; } = "piggy_bank"; // Icon name

    public string Notes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
