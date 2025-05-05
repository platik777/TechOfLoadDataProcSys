using Riok.Mapperly.Abstractions;
using UserService.Database.Entities;
using UserService.Models.DomainInterfaces;

namespace UserService.Mapper;

[Mapper]
[Obsolete("Not used any more")]
public partial class UserEntityToUserMapper : IUserEntityToUserMapper
{
    public partial IUser MapToUser(UserEntity user);
}