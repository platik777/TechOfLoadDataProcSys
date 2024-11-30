using KafkaEventsSender.Controllers;
using KafkaEventsSender.Mappers;
using KafkaEventsSender.Repositories.Rpm;
using KafkaEventsSender.Services.Kafka;
using KafkaEventsSender.Services.Rpm;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddSingleton<IKafkaSenderService, KafkaSenderService>();
builder.Services.AddSingleton<IRpmService, RpmService>();
builder.Services.AddSingleton<IRpmRepository, RpmRepository>();
builder.Services.AddSingleton<RpmEntityToRpmModelMapper>();
builder.Services.AddSingleton<RpmModelToRpmEntityMapper>();
builder.Services.AddSingleton<RpmDtoToRpmModelMapper>();

var app = builder.Build();

app.MapGrpcService<RpmServiceController>();

await app.RunAsync("http://*:5003");