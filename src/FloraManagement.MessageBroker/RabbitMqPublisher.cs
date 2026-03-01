using System.Text;
using System.Text.Json;
using FloraManagement.Contracts.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FloraManagement.MessageBroker;

/// <summary>
/// RabbitMQ message publisher implementation
/// </summary>
public class RabbitMqPublisher : IMessagePublisher, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    private async Task PublishMessageAsync<T>(T message, string routingKey) where T : FlowerMessage
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            await _channel.BasicPublishAsync(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                body: body
            );
            _logger.LogInformation(
                "Published message of type {MessageType} with routing key {RoutingKey}",
                typeof(T).Name,
                routingKey
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message of type {MessageType}", typeof(T).Name);
            throw;
        }
    }

    public RabbitMqPublisher(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqPublisher> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password
        };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false
        ).GetAwaiter().GetResult();
        _logger.LogInformation("RabbitMQ Publisher initialized successfully");
    }

    public async Task PublishCreateMessageAsync(CreateFlowerMessage message)
    {
        await PublishMessageAsync(message, _settings.CreateRoutingKey);
    }

    public async Task PublishUpdateMessageAsync(UpdateFlowerMessage message)
    {
        await PublishMessageAsync(message, _settings.UpdateRoutingKey);
    }

    public async Task PublishDeleteMessageAsync(DeleteFlowerMessage message)
    {
        await PublishMessageAsync(message, _settings.DeleteRoutingKey);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
