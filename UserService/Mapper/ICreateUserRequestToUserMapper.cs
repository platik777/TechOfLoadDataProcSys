using UserService.Models;

namespace UserService.Mapper;

public interface ICreateUserRequestToUserMapper
{ 
    User CreateUserRequestToUser(CreateUserRequest user);
}