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
        services.AddSingleton<UserCreateValidator>();
        services.AddSingleton<UserUpdateValidator>();
        services.AddScoped<UserService>();
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
            endpoints.MapGrpcService<UserServiceGrps>();
            endpoints.MapGrpcReflectionService();
        });
    }

}