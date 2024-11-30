using Grpc.Core;
using KafkaEventsSender.Services.Rpm;

namespace KafkaEventsSender.Controllers;

public class RpmServiceController : RpmService.RpmServiceBase
{
    private readonly IRpmService _rpmService;

    public RpmServiceController(IRpmService rpmService)
    {
        _rpmService = rpmService;
    }

    public override Task<RpmReply> CreateRpm(CreateRpmRequest request, ServerCallContext context)
    {
        var createdRpm = _rpmService.CreateRpm(request);

        var reply = new RpmReply
        {
            UserId = createdRpm.UserId,
            Endpoint = createdRpm.Endpoint,
            Rpm = createdRpm.Rpm
        };

        return Task.FromResult(reply);
    }

    public override Task<RpmReply> GetRpm(GetRpmRequest request, ServerCallContext context)
    {
        var retrievedRpm = _rpmService.GetRpm(request);

        var reply = new RpmReply
        {
            UserId = retrievedRpm.UserId,
            Endpoint = retrievedRpm.Endpoint,
            Rpm = retrievedRpm.Rpm
        };

        return Task.FromResult(reply);
    }

    public override Task<RpmReply> UpdateRpm(UpdateRpmRequest request, ServerCallContext context)
    {
        var updatedRpm = _rpmService.UpdateRpm(request);

        var reply = new RpmReply
        {
            UserId = updatedRpm.UserId,
            Endpoint = updatedRpm.Endpoint,
            Rpm = updatedRpm.Rpm
        };

        return Task.FromResult(reply);
    }

    public override Task<RpmReply> DeleteRpm(DeleteRpmRequest request, ServerCallContext context)
    {
        var deletedRpm = _rpmService.DeleteRpm(request);

        var reply = new RpmReply
        {
            UserId = deletedRpm.UserId,
            Endpoint = deletedRpm.Endpoint,
            Rpm = deletedRpm.Rpm
        };

        return Task.FromResult(reply);
    }
}
