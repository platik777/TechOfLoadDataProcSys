using Riok.Mapperly.Abstractions;

namespace UserService.Models.Mapper;

[Mapper]
public partial class UserMapper : IUserMapper
{
    public partial UserReply MapToUserReply(User user);
}