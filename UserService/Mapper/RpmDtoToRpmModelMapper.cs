using Riok.Mapperly.Abstractions;
using UserService.Models;

namespace UserService.Mapper;

[Mapper]
[Obsolete("Not used any more")]
public partial class RpmDtoToRpmModelMapper
{
    public partial IRpmModel CreateDtoMapToRpmModel(CreateRpmRequest request);
    public partial IRpmModel UpdateDtoMapToRpmModel(UpdateRpmRequest request);
}