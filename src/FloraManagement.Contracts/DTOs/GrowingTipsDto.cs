using System.ComponentModel.DataAnnotations;

namespace FloraManagement.Contracts.DTOs;

/// <summary>
/// DTO for preferred growing conditions
/// </summary>
public class GrowingTipsDto
{
    /// <summary>
    /// Temperature (in degrees Celsius)
    /// </summary>
    [Range(-50, 50, ErrorMessage = "Temperature must be between -50 and 50 degrees")]
    public int TemperatureCelsius { get; set; }
    /// <summary>
    /// Lighting (whether photophilous or not)
    /// </summary>
    [Required(ErrorMessage = "Lighting parameter is required")]
    public bool IsPhotophilous { get; set; }
    /// <summary>
    /// Watering (ml per week)
    /// </summary>
    [Range(1, 10000, ErrorMessage = "Watering must be between 1 and 10000 ml per week")]
    public int WateringPerWeek { get; set; }
}
