using FluentValidation;
using UserService.Controllers;
using UserService.Mapper;
using UserService.Models;
using UserService.Repository;
using UserService.Repository.Rpm;
using UserService.Services.Kafka;
using UserService.Services.Rpm;
using UserService.Services.Validators;
using RpmDtoToRpmModelMapper = UserService.Mapper.RpmDtoToRpmModelMapper;
using RpmEntityToRpmModelMapper = UserService.Mapper.RpmEntityToRpmModelMapper;
using RpmModelToRpmEntityMapper = UserService.Mapper.RpmModelToRpmEntityMapper;

namespace UserService.Services;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<DbService>();
        services.AddSingleton<IValidator<User>, UserCreateValidator>();
        services.AddSingleton<IValidator<User>, UserUpdateValidator>();
        services.AddSingleton<IUserToUserReplyMapper, UserToUserReplyMapper>();
        services.AddSingleton<IUserEntityToUserMapper, UserEntityToUserMapper>();
        services.AddSingleton<ICreateUserRequestToUserMapper, CreateUserRequestToUserMapper>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddSingleton<IRpmService, Rpm.RpmService>();
        services.AddSingleton<IRpmRepository, RpmRepository>();
        services.AddSingleton<RpmEntityToRpmModelMapper>();
        services.AddSingleton<RpmModelToRpmEntityMapper>();
        services.AddSingleton<RpmDtoToRpmModelMapper>();
        
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