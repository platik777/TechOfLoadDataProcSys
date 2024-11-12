using Grpc.Core;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Repositories;

namespace RateLimiter.Reader.Services;

public class ReaderService : IReaderService
{
    private readonly IReaderRepository _readerRepository;

    public ReaderService(IReaderRepository readerRepository)
    {
        _readerRepository = readerRepository;
    }

    public async Task<List<RateLimit>> GetRateLimits(GetRateLimitsRequest request)
    {
        try
        {
            var rateLimits = await _readerRepository.GetByAllAsync();

            return rateLimits;
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching rate limits", ex);
        }
    }

}