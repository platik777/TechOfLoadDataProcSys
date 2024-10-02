using Grpc.Core;

namespace UserService.Services;
public class UserApiService : UserService.UserServiceBase
{
    
    public override Task<UserReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        return base.GetUserById(request, context);
    }

    public override Task<UserReply> GetUserByName(GetUserByNameRequest request, ServerCallContext context)
    {
        return base.GetUserByName(request, context);
    }

    public override Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        return base.CreateUser(request, context);
    }

    public override Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        return base.UpdateUser(request, context);
    }

    public override Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return base.DeleteUser(request, context);
    }
}