using UserService.Models;

namespace UserService.Repository.Rpm;

public interface IRpmRepository
{
    RpmModel CreateRpm(RpmModel rpmModel);
    RpmModel GetRpm(string userId, string route);
    RpmModel UpdateRpm(RpmModel rpmModel);
    RpmModel DeleteRpm(string userId, string route);
}