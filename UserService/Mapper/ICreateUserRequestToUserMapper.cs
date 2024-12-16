using UserService.Models;

namespace UserService.Mapper;

public interface ICreateUserRequestToUserMapper
{ 
    IUser CreateUserRequestToUser(CreateUserRequest user);
}