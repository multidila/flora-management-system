using FloraManagement.Domain.Enums;

namespace FloraManagement.Contracts.Messages;

/// <summary>
/// Message for creating a new plant
/// </summary>
public class CreateFlowerMessage : FlowerMessage
{
    public string Name { get; set; } = string.Empty;
    public SoilType Soil { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string StemColor { get; set; } = string.Empty;
    public string LeafColor { get; set; } = string.Empty;
    public decimal AverageSize { get; set; }
    public int TemperatureCelsius { get; set; }
    public bool IsPhotophilous { get; set; }
    public int WateringPerWeek { get; set; }
    public MultiplyingType Multiplying { get; set; }

    public CreateFlowerMessage()
    {
        OperationType = "Create";
    }
}
