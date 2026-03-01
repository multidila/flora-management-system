namespace FloraManagement.Contracts.Messages;

/// <summary>
/// Message for deleting a plant
/// </summary>
public class DeleteFlowerMessage : FlowerMessage
{
    public Guid Id { get; set; }

    public DeleteFlowerMessage()
    {
        OperationType = "Delete";
    }
}
