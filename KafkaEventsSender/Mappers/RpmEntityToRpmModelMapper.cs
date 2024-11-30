using KafkaEventsSender.Models;
using KafkaEventsSender.Models.Entities;
using Riok.Mapperly.Abstractions;

namespace KafkaEventsSender.Mappers;

[Mapper]
public partial class RpmEntityToRpmModelMapper
{
    public partial RpmModel MapToModel(RpmEntity rpmEntity);
}