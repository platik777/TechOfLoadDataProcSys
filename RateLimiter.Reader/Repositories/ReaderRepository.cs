using MongoDB.Driver;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Models.Entities;
using RateLimiter.Reader.Services;

namespace RateLimiter.Reader.Repositories;

public class ReaderRepository : IReaderRepository
{
    private readonly IMongoCollection<RateLimitEntity> _rateLimits;
    private readonly IRateLimitEntityToRateLimitMapper _rateLimitMapper;

    public ReaderRepository(
        DbService mongoDbService,
        IRateLimitEntityToRateLimitMapper rateLimitMapper) 
    {
        _rateLimits = mongoDbService.GetCollection<RateLimitEntity>("rate_limits");
        _rateLimitMapper = rateLimitMapper;
    }

    public async Task<List<RateLimit>> GetByAllAsync()
    {
        try
        {
            var rateLimitEntities = await _rateLimits.Find(Builders<RateLimitEntity>.Filter.Empty).ToListAsync();
            var rateLimits = rateLimitEntities.Select(entity => _rateLimitMapper.MapToRateLimit(entity)).ToList();
            
            return rateLimits;
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching rate limits", ex);
        }
    }
}