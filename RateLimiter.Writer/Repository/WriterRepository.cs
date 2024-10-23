using MongoDB.Bson;
using MongoDB.Driver;
using RateLimiter.Writer.Models;
using RateLimiter.Writer.Models.Entities;
using RateLimiter.Writer.Services;

namespace RateLimiter.Writer.Repository;

public class WriterRepository : IWriterRepository
{
    private readonly IMongoCollection<RateLimitEntity> _rateLimits;
    private readonly IRateLimitToRateLimitEntityMapper _rateLimitEntityMapper;

    public WriterRepository(DbService mongoDbService, IRateLimitToRateLimitEntityMapper rateLimitEntityMapper)
    {
        _rateLimitEntityMapper = rateLimitEntityMapper;
        _rateLimits = mongoDbService.GetCollection<RateLimitEntity>("rate_limits");
    }

    public async Task<RateLimitEntity> GetByRouteAsync(string route, CancellationToken cancellationToken)
    {
        try
        {
            return await _rateLimits.Find(rl => rl.Route == route)
                                    .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching rate limit for route: {route}", ex);
        }
    }

    public async Task<RateLimitEntity> CreateAsync(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        var entity = _rateLimitEntityMapper.MapToRateLimitEntity(rateLimit);
        try
        {
            await _rateLimits.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating rate limit", ex);
        }
    }

    public async Task<RateLimitEntity?> UpdateAsync(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        var entity = _rateLimitEntityMapper.MapToRateLimitEntity(rateLimit);
        try
        {
            var filter = new BsonDocument("Route", entity.Route);
            var updateSettings = new BsonDocument("$set", new BsonDocument("RequestsPerMinute", entity.RequestsPerMinute));

            var result = await _rateLimits.UpdateOneAsync(filter, updateSettings, cancellationToken: cancellationToken);
            
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

    public async Task<RateLimitEntity> DeleteAsync(string route, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _rateLimits.Find(rl => rl.Route == route)
                .FirstOrDefaultAsync(cancellationToken);

            var result = await _rateLimits.DeleteOneAsync(rl => rl.Route == route, cancellationToken);

            if (result.DeletedCount > 0)
            {
                return entity;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting rate limit for route: {route}", ex);
        }
    }
}
