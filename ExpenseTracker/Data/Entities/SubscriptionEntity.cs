using SQLite;

namespace ExpenseTracker.Data.Entities;

/// <summary>
/// SQLite entity for Subscription table
/// </summary>
[Table("subscriptions")]
public class SubscriptionEntity
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	[MaxLength(200), NotNull]
	public string Name { get; set; } = string.Empty;

	[NotNull]
	public decimal Amount { get; set; }

	[MaxLength(50), NotNull]
	public string BillingCycle { get; set; } = string.Empty;

	[NotNull]
	public DateTime NextBillingDate { get; set; }

	[MaxLength(100)]
	public string Category { get; set; } = string.Empty;

	[MaxLength(500)]
	public string? Notes { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime UpdatedAt { get; set; }
}
