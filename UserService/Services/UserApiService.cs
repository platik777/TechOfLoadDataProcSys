using Grpc.Core;

namespace UserService.Services;

public class UserApiService : UserService.UserServiceBase
{
    //Должно быть типо так
    /*public override Task<UserReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
    }*/
}