using Riok.Mapperly.Abstractions;
using UserService.Models;
using UserService.Models.Entities;

namespace UserService.Mapper;

[Mapper]
[Obsolete("Not used any more")]
public partial class RpmEntityToRpmModelMapper
{
    public partial IRpmModel MapToModel(RpmEntity rpmEntity);
}