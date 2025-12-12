using SQLite;

namespace ExpenseTracker.Data.Entities;

/// <summary>
/// SQLite entity for Invoice table
/// </summary>
[Table("invoices")]
public class InvoiceEntity
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	[MaxLength(200), NotNull]
	public string Name { get; set; } = string.Empty;

	[NotNull]
	public decimal Amount { get; set; }

	[NotNull]
	public DateTime DueDate { get; set; }

	[MaxLength(50), NotNull]
	public string Status { get; set; } = "Pending";

	[MaxLength(100)]
	public string Category { get; set; } = string.Empty;

	[MaxLength(500)]
	public string? Notes { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime UpdatedAt { get; set; }
}
