using Grpc.Core;
using Grpc.Core.Interceptors;
using UserService.Services.Redis;

namespace UserService.Interceptors;

public class AuthInterceptor : Interceptor
{
    private readonly IRedisService _redisService;
    private readonly string _redisKeyPrefix = "rate_limit:";

    public AuthInterceptor(IRedisService redisService)
    {
        _redisService = redisService;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        
        if (!context.Method.StartsWith("/userService.RpmService")) return await continuation(request, context);
        
        var userId = context.RequestHeaders.GetValue("user_id");
        if (string.IsNullOrEmpty(userId))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Header 'user_id' is required."));
        }

        if (request is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Request payload cannot be null."));
        }

        
        string endpoint;
        switch (request)
        {
            case CreateRpmRequest createRpmRequest:
                endpoint = createRpmRequest.Endpoint;
                break;
            case UpdateRpmRequest updateRpmRequest:
                endpoint = updateRpmRequest.Endpoint;
                break;
            case GetRpmRequest getRpmRequest:
                endpoint = getRpmRequest.Endpoint;
                break;
            case DeleteRpmRequest deleteRpmRequest:
                endpoint = deleteRpmRequest.Endpoint;
                break;
            default:
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Unsupported request type."));
        }
            
        var key = $"{_redisKeyPrefix}{userId}:{endpoint}";
        
        var isBlocked = await _redisService.IsRequestBlockedAsync(key);
        if (isBlocked)
        {
            throw new RpcException(new Status(StatusCode.ResourceExhausted, 
                $"Request limit exceeded for endpoint '{endpoint}'."));
        }
        
        return await continuation(request, context);
    }
}
