namespace FloraManagement.Contracts.Messages;

/// <summary>
/// Base message for plant operations
/// </summary>
public abstract class FlowerMessage
{
    /// <summary>
    /// Operation type
    /// </summary>
    public string OperationType { get; set; } = string.Empty;
    /// <summary>
    /// Message creation timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
