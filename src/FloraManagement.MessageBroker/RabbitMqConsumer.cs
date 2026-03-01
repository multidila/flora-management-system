using System.Text;
using System.Text.Json;
using FloraManagement.Contracts.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FloraManagement.MessageBroker;

/// <summary>
/// RabbitMQ message consumer implementation
/// </summary>
public class RabbitMqConsumer : IMessageConsumer, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    private async Task SetupQueueAsync(string queueName, string routingKey, CancellationToken cancellationToken)
    {
        await _channel!.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
        );
        await _channel.QueueBindAsync(
            queue: queueName,
            exchange: _settings.ExchangeName,
            routingKey: routingKey,
            cancellationToken: cancellationToken
        );
    }

    private async Task ConsumeAsync(string queueName, Func<string, Task> messageHandler, CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            try
            {
                await messageHandler(message);
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
                _logger.LogInformation("Message from queue {QueueName} processed successfully", queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue {QueueName}", queueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };
        await _channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken
        );
    }

    private async Task ProcessCreateMessageAsync(string messageJson)
    {
        var message = JsonSerializer.Deserialize<CreateFlowerMessage>(messageJson);
        if (message != null && OnCreateMessageReceived != null)
        {
            await OnCreateMessageReceived(message);
        }
    }

    private async Task ProcessUpdateMessageAsync(string messageJson)
    {
        var message = JsonSerializer.Deserialize<UpdateFlowerMessage>(messageJson);
        if (message != null && OnUpdateMessageReceived != null)
        {
            await OnUpdateMessageReceived(message);
        }
    }

    private async Task ProcessDeleteMessageAsync(string messageJson)
    {
        var message = JsonSerializer.Deserialize<DeleteFlowerMessage>(messageJson);
        if (message != null && OnDeleteMessageReceived != null)
        {
            await OnDeleteMessageReceived(message);
        }
    }

    public Func<CreateFlowerMessage, Task>? OnCreateMessageReceived { get; set; }

    public Func<UpdateFlowerMessage, Task>? OnUpdateMessageReceived { get; set; }

    public Func<DeleteFlowerMessage, Task>? OnDeleteMessageReceived { get; set; }

    public RabbitMqConsumer(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqConsumer> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password
        };
        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await _channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken
        );
        await SetupQueueAsync(_settings.CreateQueueName, _settings.CreateRoutingKey, cancellationToken);
        await SetupQueueAsync(_settings.UpdateQueueName, _settings.UpdateRoutingKey, cancellationToken);
        await SetupQueueAsync(_settings.DeleteQueueName, _settings.DeleteRoutingKey, cancellationToken);
        await ConsumeAsync(_settings.CreateQueueName, ProcessCreateMessageAsync, cancellationToken);
        await ConsumeAsync(_settings.UpdateQueueName, ProcessUpdateMessageAsync, cancellationToken);
        await ConsumeAsync(_settings.DeleteQueueName, ProcessDeleteMessageAsync, cancellationToken);
        _logger.LogInformation("RabbitMQ Consumer started successfully");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ Consumer stopping");
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
