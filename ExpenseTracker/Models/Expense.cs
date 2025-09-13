<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

public abstract class Expense
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [Required]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
=======
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

public class Expense
{
	public string Id { get; set; } = string.Empty;

	[Required(ErrorMessage = "Name is required")]
	[StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
	public string Name { get; set; } = string.Empty;

	[Required(ErrorMessage = "Amount is required")]
	[Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
	public decimal Amount { get; set; }

	[Required(ErrorMessage = "Category is required")]
	[StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters")]
	public string Category { get; set; } = string.Empty;

	[Required(ErrorMessage = "Payment date is required")]
	public DateTime PaymentDate { get; set; } = DateTime.Now;

	public DateTime? DueDate { get; set; }

	public bool IsRecurring { get; set; }

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ExpenseFrequency? Frequency { get; set; }

	[StringLength(500, ErrorMessage = "Notes cannot be longer than 500 characters")]
	public string? Notes { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum ExpenseFrequency
{
	Daily,
	Weekly,
	Monthly,
	Yearly
}

public enum BillingCycle
{
	Monthly,
	Quarterly,
	Yearly
}

public enum PaymentStatus
{
	Paid,
	Pending,
	Overdue
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
