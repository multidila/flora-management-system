namespace FloraManagement.Domain.Entities;

/// <summary>
/// Visual parameters of the plant
/// </summary>
public class VisualParameters
{
    /// <summary>
    /// Stem color
    /// </summary>
    public string StemColor { get; set; } = string.Empty;
    /// <summary>
    /// Leaf color
    /// </summary>
    public string LeafColor { get; set; } = string.Empty;
    /// <summary>
    /// Average plant size (in cm)
    /// </summary>
    public decimal AverageSize { get; set; }
}
