using Riok.Mapperly.Abstractions;
using UserService.Models;
using UserService.Models.Entities;

namespace UserService.Mapper;

[Mapper]
public partial class RpmEntityToRpmModelMapper
{
    public partial RpmModel MapToModel(RpmEntity rpmEntity);
}