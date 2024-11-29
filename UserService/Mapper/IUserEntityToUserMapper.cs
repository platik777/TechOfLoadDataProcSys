using UserService.Database.Entities;
using UserService.Models;

namespace UserService.Mapper;

public interface IUserEntityToUserMapper
{
    User MapToUser(UserEntity user);
}