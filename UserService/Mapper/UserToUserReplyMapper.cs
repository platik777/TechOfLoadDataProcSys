using Riok.Mapperly.Abstractions;
using UserService.Models;

namespace UserService.Mapper;

[Mapper]
public partial class UserToUserReplyMapper : IUserToUserReplyMapper
{
    public partial UserReply MapToUserReply(User user);
}