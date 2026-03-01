using System.ComponentModel.DataAnnotations;
using FloraManagement.Domain.Enums;

namespace FloraManagement.Contracts.DTOs;

/// <summary>
/// DTO for creating a new plant (POST request)
/// </summary>
public class CreateFlowerDto
{
    /// <summary>
    /// Plant name
    /// </summary>
    [Required(ErrorMessage = "Plant name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Soil for planting
    /// </summary>
    [Required(ErrorMessage = "Soil type is required")]
    public SoilType Soil { get; set; }
    /// <summary>
    /// Plant origin
    /// </summary>
    [Required(ErrorMessage = "Origin is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Origin must be between 1 and 200 characters")]
    public string Origin { get; set; } = string.Empty;
    /// <summary>
    /// Visual parameters
    /// </summary>
    [Required(ErrorMessage = "Visual parameters are required")]
    public VisualParametersDto VisualParameters { get; set; } = new();
    /// <summary>
    /// Preferred growing conditions
    /// </summary>
    [Required(ErrorMessage = "Growing conditions are required")]
    public GrowingTipsDto GrowingTips { get; set; } = new();
    /// <summary>
    /// Multiplication method
    /// </summary>
    [Required(ErrorMessage = "Multiplication method is required")]
    public MultiplyingType Multiplying { get; set; }
}
