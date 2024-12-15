using System.Collections.Concurrent;
using UserService.Mapper;
using UserService.Models;
using UserService.Models.Entities;
using UserService.Services.Kafka;

namespace UserService.Repository.Rpm;

public class RpmRepository : IRpmRepository
{
    private readonly ConcurrentDictionary<string, RpmEntity> _storage = new();
    private readonly RpmEntityToRpmModelMapper _mapper;
    private readonly KafkaHostedService _kafkaHostedService;

    public RpmRepository(RpmEntityToRpmModelMapper mapper, KafkaHostedService kafkaHostedService)
    {
        _mapper = mapper;
        _kafkaHostedService = kafkaHostedService;
    }

    public RpmModel CreateRpm(RpmModel rpmModel)
    {
        string key = GenerateKey(rpmModel.UserId, rpmModel.Endpoint);
        var entity = new RpmEntity(rpmModel.UserId, rpmModel.Endpoint, rpmModel.Rpm);

        if (!_storage.TryAdd(key, entity))
        {
            throw new InvalidOperationException("Rpm record already exists.");
        }

        _kafkaHostedService.AddOrUpdateTask(key, rpmModel.Rpm, new
        {
            UserId = rpmModel.UserId,
            Endpoint = rpmModel.Endpoint
        });

        return _mapper.MapToModel(entity);
    }

    public RpmModel GetRpm(long userId, string route)
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

        _kafkaHostedService.AddOrUpdateTask(key, rpmModel.Rpm, new
        {
            UserId = rpmModel.UserId,
            Endpoint = rpmModel.Endpoint
        });

        return _mapper.MapToModel(entity);
    }

    public RpmModel DeleteRpm(long userId, string route)
    {
        string key = GenerateKey(userId, route);

        if (!_storage.TryRemove(key, out var entity))
        {
            throw new KeyNotFoundException("Rpm record not found.");
        }

        _kafkaHostedService.RemoveTask(key);

        return _mapper.MapToModel(entity);
    }

    private static string GenerateKey(long userId, string route) => $"{userId}_{route}";
}