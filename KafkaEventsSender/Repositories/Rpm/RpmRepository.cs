using System.Collections.Concurrent;
using KafkaEventsSender.Mappers;
using KafkaEventsSender.Models;
using KafkaEventsSender.Models.Entities;

namespace KafkaEventsSender.Repositories.Rpm;

public class RpmRepository : IRpmRepository
{
    private readonly ConcurrentDictionary<string, RpmEntity> _storage = new();
    private readonly RpmEntityToRpmModelMapper _mapper;

    public RpmRepository(RpmEntityToRpmModelMapper mapper)
    {
        _mapper = mapper;
    }

    public RpmModel CreateRpm(RpmModel rpmModel)
    {
        string key = GenerateKey(rpmModel.UserId, rpmModel.Endpoint);

        var entity = new RpmEntity(rpmModel.UserId, rpmModel.Endpoint, rpmModel.Rpm);

        if (!_storage.TryAdd(key, entity))
        {
            throw new InvalidOperationException("Rpm record already exists.");
        }

        return _mapper.MapToModel(entity);
    }

    public RpmModel GetRpm(string userId, string route)
    {
        string key = GenerateKey(userId, route);

        if (!_storage.TryGetValue(key, out var entity))
        {
            throw new KeyNotFoundException("Rpm record not found.");
        }

        return _mapper.MapToModel(entity);
    }

    public RpmModel UpdateRpm(RpmModel rpmModel)
    {
        string key = GenerateKey(rpmModel.UserId, rpmModel.Endpoint);

        if (!_storage.TryGetValue(key, out var entity))
        {
            throw new KeyNotFoundException("Rpm record not found.");
        }

        entity.Rpm = rpmModel.Rpm;

        _storage[key] = entity;

        return _mapper.MapToModel(entity);
    }

    public RpmModel DeleteRpm(string userId, string route)
    {
        string key = GenerateKey(userId, route);

        if (!_storage.TryRemove(key, out var entity))
        {
            throw new KeyNotFoundException("Rpm record not found.");
        }

        return _mapper.MapToModel(entity);
    }

    private static string GenerateKey(string userId, string route) => $"{userId}_{route}";
}
