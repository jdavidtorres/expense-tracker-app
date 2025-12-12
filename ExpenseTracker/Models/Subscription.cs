using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models;

/// <summary>
/// Represents a recurring subscription expense
/// </summary>
public class Subscription : Expense
{
    /// <summary>
    /// Gets or sets the billing cycle for the subscription
    /// </summary>
    [Required]
    [JsonPropertyName("billingCycle")]
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;

    /// <summary>
    /// Gets or sets the next billing date
    /// </summary>
    [JsonPropertyName("nextBillingDate")]
    public DateTime NextBillingDate { get; set; } = DateTime.UtcNow.AddMonths(1);

    /// <summary>
    /// Gets or sets a value indicating whether the subscription is active
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the service provider name
    /// </summary>
    [JsonPropertyName("provider")]
    public string? Provider { get; set; }
}

/// <summary>
/// Represents the billing cycle period for subscriptions
/// </summary>
public enum BillingCycle
{
    /// <summary>
    /// Billed weekly
    /// </summary>
    Weekly = 1,
    
    /// <summary>
    /// Billed monthly
    /// </summary>
    Monthly = 2,
    
    /// <summary>
    /// Billed quarterly (every 3 months)
    /// </summary>
    Quarterly = 3,
    
    /// <summary>
    /// Billed semi-annually (every 6 months)
    /// </summary>
    SemiAnnually = 6,
    
    /// <summary>
    /// Billed annually (yearly)
    /// </summary>
    Annually = 12
}