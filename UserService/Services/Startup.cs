using FluentValidation;
using UserService.Controllers;
using UserService.Models;
using UserService.Models.Mapper;
using UserService.Repository;
using UserService.Services.Utils;
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
        services.AddSingleton<IValidator<User>, UserCreateValidator>();
        services.AddSingleton<IValidator<User>, UserUpdateValidator>();
        services.AddSingleton<IUserMapper, UserMapper>();
        services.AddScoped<IUserService, UserService>();
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
            endpoints.MapGrpcReflectionService();
        });
    }

}