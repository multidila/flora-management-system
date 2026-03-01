using FloraManagement.Domain.Entities;

namespace FloraManagement.Persistence.Repositories;

/// <summary>
/// Repository interface for working with plants
/// </summary>
public interface IFlowerRepository
{
    /// <summary>
    /// Get all plants
    /// </summary>
    Task<IEnumerable<Flower>> GetAllAsync();
    /// <summary>
    /// Get plant by ID
    /// </summary>
    Task<Flower?> GetByIdAsync(Guid id);
    /// <summary>
    /// Add a new plant
    /// </summary>
    Task<Flower> CreateAsync(Flower flower);
    /// <summary>
    /// Update a plant
    /// </summary>
    Task<Flower> UpdateAsync(Flower flower);
    /// <summary>
    /// Delete a plant
    /// </summary>
    Task DeleteAsync(Guid id);
    /// <summary>
    /// Check if a plant exists
    /// </summary>
    Task<bool> ExistsAsync(Guid id);
}
