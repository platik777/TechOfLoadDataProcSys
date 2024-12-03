using UserService.Models;

namespace UserService.Services.Rpm;

public interface IRpmService
{
    RpmModel CreateRpm(CreateRpmRequest request);
    RpmModel GetRpm(GetRpmRequest request);
    RpmModel UpdateRpm(UpdateRpmRequest request);
    RpmModel DeleteRpm(DeleteRpmRequest request);
}