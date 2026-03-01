namespace FloraManagement.Domain.Entities;

/// <summary>
/// Preferred growing conditions
/// </summary>
public class GrowingTips
{
    /// <summary>
    /// Temperature (in degrees Celsius)
    /// </summary>
    public int TemperatureCelsius { get; set; }
    /// <summary>
    /// Lighting (whether photophilous or not)
    /// </summary>
    public bool IsPhotophilous { get; set; }
    /// <summary>
    /// Watering (ml per week)
    /// </summary>
    public int WateringPerWeek { get; set; }
}
