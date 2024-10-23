using MongoDB.Driver;
using RateLimiter.Writer.Models;
using RateLimiter.Writer.Models.Entities;
using RateLimiter.Writer.Services;

namespace RateLimiter.Writer.Repository;

public class WriterRepository : IWriterRepository
{
    private readonly IMongoCollection<RateLimitEntity> _rateLimits;

    public WriterRepository(DbService mongoDbService)
    {
        _rateLimits = mongoDbService.GetCollection<RateLimitEntity>("rate_limits");
    }

    public async Task<RateLimitEntity> GetByRouteAsync(string route)
    {
        try
        {
            return await _rateLimits.Find(rl => rl.Route == route).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching rate limit for route: {route}", ex);
        }
    }

    public async Task<RateLimitEntity> CreateAsync(RateLimit rateLimit)
    {
        var entity = new RateLimitEntity(rateLimit.Route, rateLimit.RequestsPerMinute);
        try
        {
            await _rateLimits.InsertOneAsync(entity);
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating rate limit", ex);
        }
    }

    public async Task<RateLimitEntity?> UpdateAsync(RateLimit rateLimit)
    {
        var entity = new RateLimitEntity(rateLimit.Route, rateLimit.RequestsPerMinute);
        try
        {
            var result = await _rateLimits.ReplaceOneAsync(rl => rl.Route == rateLimit.Route, entity);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return entity;
            }
            return null; 
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating rate limit for route: {rateLimit.Route}", ex);
        }
    }

    public async Task DeleteAsync(string route)
    {
        try
        {
            await _rateLimits.DeleteOneAsync(rl => rl.Route == route);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting rate limit for route: {route}", ex);
        }
    }
}
