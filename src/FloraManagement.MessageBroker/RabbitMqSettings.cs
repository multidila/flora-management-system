namespace FloraManagement.MessageBroker;

/// <summary>
/// RabbitMQ configuration settings
/// </summary>
public class RabbitMqSettings
{
    /// <summary>
    /// RabbitMQ host
    /// </summary>
    public string Host { get; set; } = "localhost";
    /// <summary>
    /// RabbitMQ port
    /// </summary>
    public int Port { get; set; } = 5672;
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = "guest";
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = "guest";
    /// <summary>
    /// Exchange name
    /// </summary>
    public string ExchangeName { get; set; } = "flowers.exchange";
    /// <summary>
    /// Create queue name
    /// </summary>
    public string CreateQueueName { get; set; } = "flowers.create.queue";
    /// <summary>
    /// Update queue name
    /// </summary>
    public string UpdateQueueName { get; set; } = "flowers.update.queue";
    /// <summary>
    /// Delete queue name
    /// </summary>
    public string DeleteQueueName { get; set; } = "flowers.delete.queue";
    /// <summary>
    /// Routing key for create operations
    /// </summary>
    public string CreateRoutingKey { get; set; } = "flower.create";
    /// <summary>
    /// Routing key for update operations
    /// </summary>
    public string UpdateRoutingKey { get; set; } = "flower.update";
    /// <summary>
    /// Routing key for delete operations
    /// </summary>
    public string DeleteRoutingKey { get; set; } = "flower.delete";
}
