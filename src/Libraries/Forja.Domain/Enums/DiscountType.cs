namespace Forja.Domain.Enums;

/// <summary>
/// Represents the type of discount that can be applied.
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// A discount that is a percentage of the original price.
    /// </summary>
    Percentage,

    /// <summary>
    /// A discount that is a fixed amount subtracted from the original price.
    /// </summary>
    Fixed,
}