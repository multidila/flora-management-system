using System.ComponentModel.DataAnnotations;

namespace FloraManagement.Contracts.DTOs;

/// <summary>
/// DTO for visual parameters of the plant
/// </summary>
public class VisualParametersDto
{
    /// <summary>
    /// Stem color
    /// </summary>
    [Required(ErrorMessage = "Stem color is required")]
    [StringLength(50, ErrorMessage = "Maximum length is 50 characters")]
    public string StemColor { get; set; } = string.Empty;
    /// <summary>
    /// Leaf color
    /// </summary>
    [Required(ErrorMessage = "Leaf color is required")]
    [StringLength(50, ErrorMessage = "Maximum length is 50 characters")]
    public string LeafColor { get; set; } = string.Empty;
    /// <summary>
    /// Average plant size (in cm)
    /// </summary>
    [Range(0.1, 1000, ErrorMessage = "Size must be between 0.1 and 1000 cm")]
    public decimal AverageSize { get; set; }
}
