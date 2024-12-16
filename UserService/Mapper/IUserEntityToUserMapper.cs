using UserService.Database.Entities;
using UserService.Models;

namespace UserService.Mapper;

public interface IUserEntityToUserMapper
{
    IUser MapToUser(UserEntity user);
}