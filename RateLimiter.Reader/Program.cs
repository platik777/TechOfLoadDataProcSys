using RateLimiter.Reader.Controllers;
using RateLimiter.Reader.Mappers;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Repositories;
using RateLimiter.Reader.Services;
using RateLimiter.Reader.Services.Kafka;
using RateLimiter.Reader.Services.RateLimits;
using RateLimiter.Reader.Services.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddSingleton<IRateLimitToRateLimitReplyMapper, RateLimitToRateLimitReplyMapper>();
builder.Services.AddSingleton<IRateLimitEntityToRateLimitMapper, RateLimitEntityToRateLimitMapper>();
builder.Services.AddSingleton<IRateLimitToRateLimitEntityMapper, RateLimitToRateLimitEntityMapper>();
builder.Services.AddSingleton<DbService>();
builder.Services.AddSingleton<IReaderRepository, ReaderRepository>();
builder.Services.AddSingleton<IReaderService, ReaderService>();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<IRateLimitChecker, RateLimitChecker>();

builder.Services.AddHostedService<HostedService>();
builder.Services.AddHostedService<KafkaConsumer>();

var app = builder.Build();

app.MapGrpcService<ReaderServiceController>();

await app.RunAsync("http://*:5000");