using Riok.Mapperly.Abstractions;
using UserService.Database.Entities;
using UserService.Models;

namespace UserService.Mapper;

[Mapper]
public partial class UserEntityToUserMapper : IUserEntityToUserMapper
{
    public partial IUser MapToUser(UserEntity user);
}