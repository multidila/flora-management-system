using FloraManagement.Contracts.DTOs;
using FloraManagement.Contracts.Messages;
using FloraManagement.MessageBroker;
using FloraManagement.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FloraManagement.API.Controllers;

/// <summary>
/// Controller for managing greenhouse plants
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FlowersController : ControllerBase
{
    private readonly IFlowerRepository _repository;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<FlowersController> _logger;

    public FlowersController(
        IFlowerRepository repository,
        IMessagePublisher publisher,
        ILogger<FlowersController> logger)
    {
        _repository = repository;
        _publisher = publisher;
        _logger = logger;
    }

    /// <summary>
    /// Get all plants
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FlowerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FlowerDto>>> GetAll()
    {
        _logger.LogInformation("GET api/flowers - Fetching all flowers");
        var flowers = await _repository.GetAllAsync();
        var flowerDtos = flowers.Select(f => new FlowerDto
        {
            Id = f.Id,
            Name = f.Name,
            Soil = f.Soil,
            Origin = f.Origin,
            VisualParameters = new VisualParametersDto
            {
                StemColor = f.VisualParameters.StemColor,
                LeafColor = f.VisualParameters.LeafColor,
                AverageSize = f.VisualParameters.AverageSize
            },
            GrowingTips = new GrowingTipsDto
            {
                TemperatureCelsius = f.GrowingTips.TemperatureCelsius,
                IsPhotophilous = f.GrowingTips.IsPhotophilous,
                WateringPerWeek = f.GrowingTips.WateringPerWeek
            },
            Multiplying = f.Multiplying,
            CreatedAt = f.CreatedAt,
            UpdatedAt = f.UpdatedAt
        });
        return Ok(flowerDtos);
    }

    /// <summary>
    /// Get plant by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FlowerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlowerDto>> GetById(Guid id)
    {
        _logger.LogInformation("GET api/flowers/{FlowerId} - Fetching flower", id);
        var flower = await _repository.GetByIdAsync(id);
        if (flower == null)
        {
            _logger.LogWarning("Flower with ID {FlowerId} not found", id);
            return NotFound(new { message = $"Plant with ID {id} not found" });
        }
        var flowerDto = new FlowerDto
        {
            Id = flower.Id,
            Name = flower.Name,
            Soil = flower.Soil,
            Origin = flower.Origin,
            VisualParameters = new VisualParametersDto
            {
                StemColor = flower.VisualParameters.StemColor,
                LeafColor = flower.VisualParameters.LeafColor,
                AverageSize = flower.VisualParameters.AverageSize
            },
            GrowingTips = new GrowingTipsDto
            {
                TemperatureCelsius = flower.GrowingTips.TemperatureCelsius,
                IsPhotophilous = flower.GrowingTips.IsPhotophilous,
                WateringPerWeek = flower.GrowingTips.WateringPerWeek
            },
            Multiplying = flower.Multiplying,
            CreatedAt = flower.CreatedAt,
            UpdatedAt = flower.UpdatedAt
        };
        return Ok(flowerDto);
    }

    /// <summary>
    /// Create a new plant
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] CreateFlowerDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for CreateFlowerDto");
            return BadRequest(ModelState);
        }
        _logger.LogInformation("POST api/flowers - Creating new flower: {FlowerName}", dto.Name);
        var message = new CreateFlowerMessage
        {
            Name = dto.Name,
            Soil = dto.Soil,
            Origin = dto.Origin,
            StemColor = dto.VisualParameters.StemColor,
            LeafColor = dto.VisualParameters.LeafColor,
            AverageSize = dto.VisualParameters.AverageSize,
            TemperatureCelsius = dto.GrowingTips.TemperatureCelsius,
            IsPhotophilous = dto.GrowingTips.IsPhotophilous,
            WateringPerWeek = dto.GrowingTips.WateringPerWeek,
            Multiplying = dto.Multiplying
        };
        await _publisher.PublishCreateMessageAsync(message);
        _logger.LogInformation("Create message sent to queue for flower: {FlowerName}", dto.Name);
        return Accepted(new { message = "Plant creation request sent to processing queue" });
    }

    /// <summary>
    /// Update a plant
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateFlowerDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for UpdateFlowerDto");
            return BadRequest(ModelState);
        }
        var exists = await _repository.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Flower with ID {FlowerId} not found for update", id);
            return NotFound(new { message = $"Plant with ID {id} not found" });
        }
        _logger.LogInformation("PUT api/flowers/{FlowerId} - Updating flower", id);
        var message = new UpdateFlowerMessage
        {
            Id = id,
            Name = dto.Name,
            Soil = dto.Soil,
            Origin = dto.Origin,
            StemColor = dto.VisualParameters.StemColor,
            LeafColor = dto.VisualParameters.LeafColor,
            AverageSize = dto.VisualParameters.AverageSize,
            TemperatureCelsius = dto.GrowingTips.TemperatureCelsius,
            IsPhotophilous = dto.GrowingTips.IsPhotophilous,
            WateringPerWeek = dto.GrowingTips.WateringPerWeek,
            Multiplying = dto.Multiplying
        };
        await _publisher.PublishUpdateMessageAsync(message);
        _logger.LogInformation("Update message sent to queue for flower ID: {FlowerId}", id);
        return Accepted(new { message = "Plant update request sent to processing queue" });
    }

    /// <summary>
    /// Delete a plant
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var exists = await _repository.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Flower with ID {FlowerId} not found for deletion", id);
            return NotFound(new { message = $"Plant with ID {id} not found" });
        }
        _logger.LogInformation("DELETE api/flowers/{FlowerId} - Deleting flower", id);
        var message = new DeleteFlowerMessage
        {
            Id = id
        };
        await _publisher.PublishDeleteMessageAsync(message);
        _logger.LogInformation("Delete message sent to queue for flower ID: {FlowerId}", id);
        return Accepted(new { message = "Plant deletion request sent to processing queue" });
    }
}
