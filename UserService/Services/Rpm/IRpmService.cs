using UserService.Models;

namespace UserService.Services.Rpm;

public interface IRpmService
{
    IRpmModel CreateRpm(IRpmModel request);
    IRpmModel GetRpm(long userId, string endpoint);
    IRpmModel UpdateRpm(IRpmModel request);
    IRpmModel DeleteRpm(long userId, string endpoint);
}