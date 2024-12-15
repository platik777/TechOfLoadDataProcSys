using UserService.Models;

namespace UserService.Repository.Rpm;

public interface IRpmRepository
{
    RpmModel CreateRpm(RpmModel rpmModel);
    RpmModel GetRpm(long userId, string route);
    RpmModel UpdateRpm(RpmModel rpmModel);
    RpmModel DeleteRpm(long userId, string route);
}