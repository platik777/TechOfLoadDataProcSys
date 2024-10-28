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
    private readonly IRateLimitEntityToRateLimitMapper _rateLimitMapper;

    public WriterRepository(DbService mongoDbService, IRateLimitToRateLimitEntityMapper rateLimitEntityMapper, IRateLimitEntityToRateLimitMapper rateLimitMapper)
    {
        _rateLimitEntityMapper = rateLimitEntityMapper;
        _rateLimits = mongoDbService.GetCollection<RateLimitEntity>("rate_limits");
        _rateLimitMapper = rateLimitMapper;
    }

    public async Task<RateLimit> GetByRouteAsync(string route, CancellationToken cancellationToken)
    {
        try
        {
            return _rateLimitMapper.MapToRateLimit(await _rateLimits.Find(rl => rl.Route == route)
                                    .FirstOrDefaultAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching rate limit for route: {route}", ex);
        }
    }

    public async Task<RateLimit> CreateAsync(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        var entity = _rateLimitEntityMapper.MapToRateLimitEntity(rateLimit);
        try
        {
            await _rateLimits.InsertOneAsync(entity, cancellationToken: cancellationToken);
            return _rateLimitMapper.MapToRateLimit(entity);
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating rate limit", ex);
        }
    }

    public async Task<RateLimit?> UpdateAsync(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        var entity = _rateLimitEntityMapper.MapToRateLimitEntity(rateLimit);
        try
        {
            var filter = Builders<RateLimitEntity>.Filter.Eq(rl => rl.Route, entity.Route);
            var update = Builders<RateLimitEntity>.Update.Set(rl => rl.RequestsPerMinute, entity.RequestsPerMinute);

            var result = await _rateLimits.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return _rateLimitMapper.MapToRateLimit(entity);
            }
            return null; 
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating rate limit for route: {rateLimit.Route}", ex);
        }
    }

    public async Task<RateLimit?> DeleteAsync(string route, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _rateLimits.Find(rl => rl.Route == route)
                .FirstOrDefaultAsync(cancellationToken);

            var result = await _rateLimits.DeleteOneAsync(rl => rl.Route == route, cancellationToken);

            if (result.DeletedCount > 0)
            {
                return _rateLimitMapper.MapToRateLimit(entity);
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting rate limit for route: {route}", ex);
        }
    }
}
