using RateLimiter.Writer.Controllers;
using RateLimiter.Writer.Mapper;
using RateLimiter.Writer.Models;
using RateLimiter.Writer.Repository;

namespace RateLimiter.Writer.Services;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        
        services.AddSingleton<IWriterRepository, WriterRepository>();
        services.AddSingleton<IRateLimitToRateLimitReplyMapper, RateLimitToRateLimitReplyMapper>();
        services.AddSingleton<DbService>();
        services.AddScoped<IWriterService, WriterService>();
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
            endpoints.MapGrpcService<WriterServiceController>();
            endpoints.MapGrpcReflectionService();
        });
    }
}