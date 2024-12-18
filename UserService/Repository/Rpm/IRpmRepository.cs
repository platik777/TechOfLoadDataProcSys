using UserService.Models;

namespace UserService.Repository.Rpm;

public interface IRpmRepository
{
    IRpmModel CreateRpm(IRpmModel rpmModel);
    IRpmModel GetRpm(long userId, string route);
    IRpmModel UpdateRpm(IRpmModel rpmModel);
    IRpmModel DeleteRpm(long userId, string route);
}