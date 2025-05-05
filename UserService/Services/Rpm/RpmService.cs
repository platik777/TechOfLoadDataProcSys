using UserService.Models;
using UserService.Repository.Rpm;

namespace UserService.Services.Rpm;

public class RpmService : IRpmService
{
    private readonly IRpmRepository _rpmRepository;

    public RpmService(IRpmRepository rpmRepository)
    {
        _rpmRepository = rpmRepository;
    }

    public IRpmModel CreateRpm(IRpmModel request)
    {
        return _rpmRepository.CreateRpm(request);
    }

    public IRpmModel GetRpm(long userId, string endpoint)
    {
        return _rpmRepository.GetRpm(userId, endpoint);
    }

    public IRpmModel UpdateRpm(IRpmModel request)
    {
        return _rpmRepository.UpdateRpm(request);
    }

    public IRpmModel DeleteRpm(long userId, string endpoint)
    {
        return _rpmRepository.DeleteRpm(userId, endpoint);
    }
}