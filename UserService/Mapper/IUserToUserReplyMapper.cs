using UserService.Models;
using UserService.Models.DomainInterfaces;

namespace UserService.Mapper;


public interface IUserToUserReplyMapper
{
    UserReply MapToUserReply(IUser user);
}