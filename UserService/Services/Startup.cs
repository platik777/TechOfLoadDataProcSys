using FluentValidation;
using UserService.Controllers;
using UserService.Interceptors;
using UserService.Mapper;
using UserService.Models.DomainInterfaces;
using UserService.Repository;
using UserService.Repository.Rpm;
using UserService.Services.Kafka;
using UserService.Services.Redis;
using UserService.Services.Rpm;
using UserService.Services.Validators;

namespace UserService.Services;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<DbService>();
        services.AddSingleton<IValidator<IUser>, UserCreateValidator>();
        services.AddSingleton<IValidator<IUser>, UserUpdateValidator>();
        services.AddSingleton<IUserToUserReplyMapper, UserToUserReplyMapper>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddSingleton<IRpmService, Rpm.RpmService>();
        services.AddSingleton<IRpmRepository, RpmRepository>();
        services.AddMemoryCache();
        
        services.AddSingleton<IRedisService, RedisService>();
        
        services.AddSingleton<AuthInterceptor>();
        services.AddGrpc(options =>
        {
            
            options.Interceptors.Add<AuthInterceptor>();
        });
        
        services.AddSingleton<KafkaHostedService>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var bootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers");
            return new KafkaHostedService(bootstrapServers);
        });

        services.AddHostedService(provider => provider.GetRequiredService<KafkaHostedService>());

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<UserServiceController>();
            endpoints.MapGrpcService<RpmServiceController>();
            endpoints.MapGrpcReflectionService();
        });
    }

}