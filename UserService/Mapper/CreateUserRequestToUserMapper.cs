using Riok.Mapperly.Abstractions;
using UserService.Models.DomainInterfaces;

namespace UserService.Mapper;

[Mapper]
[Obsolete("Not used any more")]
public partial class CreateUserRequestToUserMapper : ICreateUserRequestToUserMapper
{
    public partial IUser CreateUserRequestToUser(CreateUserRequest user);
}