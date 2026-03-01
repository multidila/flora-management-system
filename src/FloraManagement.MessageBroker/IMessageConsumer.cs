namespace FloraManagement.MessageBroker;

/// <summary>
/// Interface for consuming messages from RabbitMQ
/// </summary>
public interface IMessageConsumer
{
    /// <summary>
    /// Start listening for messages
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Stop listening for messages
    /// </summary>
    Task StopAsync(CancellationToken cancellationToken);
}
