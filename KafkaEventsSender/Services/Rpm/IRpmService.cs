using KafkaEventsSender.Models;

namespace KafkaEventsSender.Services.Rpm;

public interface IRpmService
{
    RpmModel CreateRpm(CreateRpmRequest request);
    RpmModel GetRpm(GetRpmRequest request);
    RpmModel UpdateRpm(UpdateRpmRequest request);
    RpmModel DeleteRpm(DeleteRpmRequest request);
}