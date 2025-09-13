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
