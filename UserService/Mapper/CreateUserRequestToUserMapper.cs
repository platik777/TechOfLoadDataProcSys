using Riok.Mapperly.Abstractions;
using UserService.Models;

namespace UserService.Mapper;

[Mapper]
public partial class CreateUserRequestToUserMapper : ICreateUserRequestToUserMapper
{
    public partial IUser CreateUserRequestToUser(CreateUserRequest user);
}