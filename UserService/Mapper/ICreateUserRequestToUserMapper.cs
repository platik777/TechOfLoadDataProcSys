using UserService.Models.DomainInterfaces;

namespace UserService.Mapper;

[Obsolete("Not used any more")]
public interface ICreateUserRequestToUserMapper
{ 
    IUser CreateUserRequestToUser(CreateUserRequest user);
}