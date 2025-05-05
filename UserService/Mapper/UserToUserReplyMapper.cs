using Riok.Mapperly.Abstractions;
using UserService.Models.DomainInterfaces;

namespace UserService.Mapper;

[Mapper]
public partial class UserToUserReplyMapper : IUserToUserReplyMapper
{
    public partial UserReply MapToUserReply(IUser user);
}