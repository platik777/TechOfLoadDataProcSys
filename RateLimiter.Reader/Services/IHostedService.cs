namespace RateLimiter.Reader.Services;

public interface IHostedService
{
    Task StartHostedServiceAsync(CancellationToken cancellationToken);
    Task StopHostedServiceAsync(CancellationToken cancellationToken);

}