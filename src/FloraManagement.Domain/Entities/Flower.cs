using FloraManagement.Domain.Enums;

namespace FloraManagement.Domain.Entities;

/// <summary>
/// Greenhouse plant (root element)
/// </summary>
public class Flower
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Plant name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Soil for planting
    /// </summary>
    public SoilType Soil { get; set; }
    /// <summary>
    /// Plant origin
    /// </summary>
    public string Origin { get; set; } = string.Empty;
    /// <summary>
    /// Visual parameters
    /// </summary>
    public VisualParameters VisualParameters { get; set; } = new();
    /// <summary>
    /// Preferred growing conditions
    /// </summary>
    public GrowingTips GrowingTips { get; set; } = new();
    /// <summary>
    /// Multiplication method
    /// </summary>
    public MultiplyingType Multiplying { get; set; }
    /// <summary>
    /// Record creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
