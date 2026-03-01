using FloraManagement.Contracts.Messages;

namespace FloraManagement.MessageBroker;

/// <summary>
/// Interface for publishing messages to RabbitMQ
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publish plant creation message
    /// </summary>
    Task PublishCreateMessageAsync(CreateFlowerMessage message);
    /// <summary>
    /// Publish plant update message
    /// </summary>
    Task PublishUpdateMessageAsync(UpdateFlowerMessage message);
    /// <summary>
    /// Publish plant deletion message
    /// </summary>
    Task PublishDeleteMessageAsync(DeleteFlowerMessage message);
}
