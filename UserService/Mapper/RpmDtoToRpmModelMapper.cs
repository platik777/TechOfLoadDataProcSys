using Riok.Mapperly.Abstractions;
using UserService.Models;

namespace UserService.Mapper;

[Mapper]
public partial class RpmDtoToRpmModelMapper
{
    public partial RpmModel CreateDtoMapToRpmModel(CreateRpmRequest request);
    public partial RpmModel UpdateDtoMapToRpmModel(UpdateRpmRequest request);
}