using RateLimiter.Reader.Controllers;
using RateLimiter.Reader.Mappers;
using RateLimiter.Reader.Models;
using RateLimiter.Reader.Repositories;
using RateLimiter.Reader.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<IReaderRepository, ReaderRepository>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddSingleton<IRateLimitToRateLimitReplyMapper, RateLimitToRateLimitReplyMapper>();
builder.Services.AddSingleton<IRateLimitEntityToRateLimitMapper, RateLimitEntityToRateLimitMapper>();
builder.Services.AddSingleton<IRateLimitToRateLimitEntityMapper, RateLimitToRateLimitEntityMapper>();
builder.Services.AddSingleton<DbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ReaderServiceController>();

await app.RunAsync("http://*:5000");