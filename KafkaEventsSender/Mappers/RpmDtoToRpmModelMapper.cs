using KafkaEventsSender.Models;
using Riok.Mapperly.Abstractions;

namespace KafkaEventsSender.Mappers;

[Mapper]
public partial class RpmDtoToRpmModelMapper
{
    public partial RpmModel CreateDtoMapToRpmModel(CreateRpmRequest request);
    public partial RpmModel UpdateDtoMapToRpmModel(UpdateRpmRequest request);
}