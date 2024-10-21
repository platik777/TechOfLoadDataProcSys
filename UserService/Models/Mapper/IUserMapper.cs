namespace UserService.Models.Mapper;

public interface IUserMapper
{
    UserReply MapToUserReply(User user);
}