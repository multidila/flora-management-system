using FloraManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloraManagement.Persistence.Repositories;

/// <summary>
/// Repository implementation for working with plants
/// </summary>
public class FlowerRepository : IFlowerRepository
{
    private readonly FlowerDbContext _context;
    private readonly ILogger<FlowerRepository> _logger;

    public FlowerRepository(FlowerDbContext context, ILogger<FlowerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Flower>> GetAllAsync()
    {
        _logger.LogInformation("Getting all flowers from database");
        return await _context.Flowers.ToListAsync();
    }

    public async Task<Flower?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting flower with ID {FlowerId} from database", id);
        return await _context.Flowers.FindAsync(id);
    }

    public async Task<Flower> CreateAsync(Flower flower)
    {
        flower.CreatedAt = DateTime.UtcNow;
        flower.UpdatedAt = DateTime.UtcNow;
        _logger.LogInformation("Creating new flower: {FlowerName}", flower.Name);
        await _context.Flowers.AddAsync(flower);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Flower created successfully with ID {FlowerId}", flower.Id);
        return flower;
    }

    public async Task<Flower> UpdateAsync(Flower flower)
    {
        flower.UpdatedAt = DateTime.UtcNow;
        _logger.LogInformation("Updating flower with ID {FlowerId}", flower.Id);
        _context.Flowers.Update(flower);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Flower updated successfully");
        return flower;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting flower with ID {FlowerId}", id);
        var flower = await _context.Flowers.FindAsync(id);
        if (flower != null)
        {
            _context.Flowers.Remove(flower);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Flower deleted successfully");
        }
        else
        {
            _logger.LogWarning("Flower with ID {FlowerId} not found", id);
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Flowers.AnyAsync(f => f.Id == id);
    }
}
