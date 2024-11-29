using Riok.Mapperly.Abstractions;
using UserService.Models;

namespace UserService.Mapper;

[Mapper]
public partial class CreateUserRequestToUserMapper : ICreateUserRequestToUserMapper
{
    public partial User CreateUserRequestToUser(CreateUserRequest user);
}