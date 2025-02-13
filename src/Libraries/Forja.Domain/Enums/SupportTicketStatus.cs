namespace Forja.Domain.Enums;

/// <summary>
/// Represents the status of a support ticket.
/// </summary>
public enum SupportTicketStatus
{
    /// <summary>
    /// The support ticket is open and requires attention.
    /// </summary>
    [Display(Name = "Open")]
    Open,

    /// <summary>
    /// The support ticket is pending and awaiting further action.
    /// </summary>
    [Display(Name = "Pending")]
    Pending,

    /// <summary>
    /// The support ticket is closed and no further action is required.
    /// </summary>
    [Display(Name = "Closed")]
    Closed
}