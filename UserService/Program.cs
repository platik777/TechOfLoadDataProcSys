using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<UserApiService>();

await app.RunAsync("http://*:5002");