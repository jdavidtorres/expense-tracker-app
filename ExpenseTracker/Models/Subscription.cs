<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

public class Subscription : Expense
{
    [Required]
    [JsonPropertyName("billingCycle")]
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;

    [JsonPropertyName("nextBillingDate")]
    public DateTime NextBillingDate { get; set; } = DateTime.UtcNow.AddMonths(1);

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = true;

    [JsonPropertyName("provider")]
    public string? Provider { get; set; }
}

public enum BillingCycle
{
    Weekly = 1,
    Monthly = 2,
    Quarterly = 3,
    SemiAnnually = 6,
    Annually = 12
}
=======
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

public class Subscription : Expense
{
	[JsonPropertyName("type")]
	public string Type { get; set; } = "subscription";

	[Required(ErrorMessage = "Billing cycle is required")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;

	[Required(ErrorMessage = "Start date is required")]
	public DateTime StartDate { get; set; } = DateTime.Now;

	[Required(ErrorMessage = "Next billing date is required")]
	public DateTime NextBillingDate { get; set; } = DateTime.Now.AddMonths(1);

	public DateTime? EndDate { get; set; }

	public bool IsActive { get; set; } = true;

	[StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
	public string Description { get; set; } = string.Empty;
}
>>>>>>> 95edd7384477a9a46f3d2218ed5d5b0eff5ce133
