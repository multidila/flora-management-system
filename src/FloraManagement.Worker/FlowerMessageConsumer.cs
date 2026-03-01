using FloraManagement.Contracts.Messages;
using FloraManagement.Domain.Entities;
using FloraManagement.MessageBroker;
using FloraManagement.Persistence.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FloraManagement.Worker;

/// <summary>
/// Background service for processing messages from RabbitMQ
/// </summary>
public class FlowerMessageConsumer : BackgroundService
{
    private readonly IMessageConsumer _consumer;
    private readonly IFlowerRepository _repository;
    private readonly ILogger<FlowerMessageConsumer> _logger;

    private async Task HandleCreateMessageAsync(CreateFlowerMessage message)
    {
        try
        {
            _logger.LogInformation("Processing CreateFlowerMessage for flower: {FlowerName}", message.Name);
            var flower = new Flower
            {
                Id = Guid.NewGuid(),
                Name = message.Name,
                Soil = message.Soil,
                Origin = message.Origin,
                VisualParameters = new VisualParameters
                {
                    StemColor = message.StemColor,
                    LeafColor = message.LeafColor,
                    AverageSize = message.AverageSize
                },
                GrowingTips = new GrowingTips
                {
                    TemperatureCelsius = message.TemperatureCelsius,
                    IsPhotophilous = message.IsPhotophilous,
                    WateringPerWeek = message.WateringPerWeek
                },
                Multiplying = message.Multiplying
            };
            await _repository.CreateAsync(flower);
            _logger.LogInformation("Flower created successfully with ID: {FlowerId}", flower.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing CreateFlowerMessage");
            throw;
        }
    }

    private async Task HandleUpdateMessageAsync(UpdateFlowerMessage message)
    {
        try
        {
            _logger.LogInformation("Processing UpdateFlowerMessage for flower ID: {FlowerId}", message.Id);
            var existingFlower = await _repository.GetByIdAsync(message.Id);
            if (existingFlower == null)
            {
                _logger.LogWarning("Flower with ID {FlowerId} not found for update", message.Id);
                return;
            }
            existingFlower.Name = message.Name;
            existingFlower.Soil = message.Soil;
            existingFlower.Origin = message.Origin;
            existingFlower.VisualParameters.StemColor = message.StemColor;
            existingFlower.VisualParameters.LeafColor = message.LeafColor;
            existingFlower.VisualParameters.AverageSize = message.AverageSize;
            existingFlower.GrowingTips.TemperatureCelsius = message.TemperatureCelsius;
            existingFlower.GrowingTips.IsPhotophilous = message.IsPhotophilous;
            existingFlower.GrowingTips.WateringPerWeek = message.WateringPerWeek;
            existingFlower.Multiplying = message.Multiplying;
            await _repository.UpdateAsync(existingFlower);
            _logger.LogInformation("Flower updated successfully: {FlowerId}", message.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UpdateFlowerMessage");
            throw;
        }
    }

    private async Task HandleDeleteMessageAsync(DeleteFlowerMessage message)
    {
        try
        {
            _logger.LogInformation("Processing DeleteFlowerMessage for flower ID: {FlowerId}", message.Id);
            await _repository.DeleteAsync(message.Id);
            _logger.LogInformation("Flower deleted successfully: {FlowerId}", message.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing DeleteFlowerMessage");
            throw;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FlowerMessageConsumer is starting");
        if (_consumer is RabbitMqConsumer rabbitConsumer)
        {
            rabbitConsumer.OnCreateMessageReceived = HandleCreateMessageAsync;
            rabbitConsumer.OnUpdateMessageReceived = HandleUpdateMessageAsync;
            rabbitConsumer.OnDeleteMessageReceived = HandleDeleteMessageAsync;
        }
        await _consumer.StartAsync(stoppingToken);
        _logger.LogInformation("FlowerMessageConsumer started and listening for messages");
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public FlowerMessageConsumer(
        IMessageConsumer consumer,
        IFlowerRepository repository,
        ILogger<FlowerMessageConsumer> logger)
    {
        _consumer = consumer;
        _repository = repository;
        _logger = logger;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("FlowerMessageConsumer is stopping");
        await _consumer.StopAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
