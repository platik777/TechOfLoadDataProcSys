using UserService.Repository;
using UserService.Services.Utils;

namespace UserService.Services;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<DbService>();
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
            endpoints.MapGrpcService<UserServiceImpl>();
            endpoints.MapGrpcReflectionService();
        });
    }

}