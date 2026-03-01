using FloraManagement.Domain.Enums;

namespace FloraManagement.Contracts.DTOs;

/// <summary>
/// DTO for displaying a plant (used in GET requests)
/// </summary>
public class FlowerDto
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
    public VisualParametersDto VisualParameters { get; set; } = new();
    /// <summary>
    /// Preferred growing conditions
    /// </summary>
    public GrowingTipsDto GrowingTips { get; set; } = new();
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
