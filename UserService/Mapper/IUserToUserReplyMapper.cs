using UserService.Models;

namespace UserService.Mapper;

public interface IUserToUserReplyMapper
{
    UserReply MapToUserReply(User user);
}