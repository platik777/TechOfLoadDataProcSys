using UserService.Database.Entities;
using UserService.Models.DomainInterfaces;

namespace UserService.Mapper;

[Obsolete("Not used any more")]
public interface IUserEntityToUserMapper
{
    IUser MapToUser(UserEntity user);
}