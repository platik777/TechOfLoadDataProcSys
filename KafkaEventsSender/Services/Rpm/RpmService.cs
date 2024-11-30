using KafkaEventsSender.Mappers;
using KafkaEventsSender.Models;
using KafkaEventsSender.Repositories.Rpm;

namespace KafkaEventsSender.Services.Rpm;

public class RpmService : IRpmService
{
    private readonly IRpmRepository _rpmRepository;
    private readonly RpmDtoToRpmModelMapper _mapper;

    public RpmService(IRpmRepository rpmRepository, RpmDtoToRpmModelMapper mapper)
    {
        _rpmRepository = rpmRepository;
        _mapper = mapper;
    }

    public RpmModel CreateRpm(CreateRpmRequest request)
    {
        return _rpmRepository.CreateRpm(_mapper.CreateDtoMapToRpmModel(request));
    }

    public RpmModel GetRpm(GetRpmRequest request)
    {
        return _rpmRepository.GetRpm(request.UserId, request.Endpoint);
    }

    public RpmModel UpdateRpm(UpdateRpmRequest request)
    {
        return _rpmRepository.UpdateRpm(_mapper.UpdateDtoMapToRpmModel(request));
    }

    public RpmModel DeleteRpm(DeleteRpmRequest request)
    {
        return _rpmRepository.DeleteRpm(request.UserId, request.Endpoint);
    }
}