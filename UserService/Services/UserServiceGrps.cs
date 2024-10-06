using Grpc.Core;

namespace UserService.Services;

public class UserServiceGrps : global::UserService.UserService.UserServiceBase
{
    private readonly UserService _userService;

    public UserServiceGrps(UserService userService)
    {
        _userService = userService;
    }
    
    public override async Task<UserListReply> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
    {
        return await _userService.GetAllUsers();
    }

    public override async Task<UserReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        return await _userService.GetUserById(request);
    }

    public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        return await _userService.CreateUser(request);
    }
    
    public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        return await _userService.UpdateUser(request);
    }
    
    public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return await _userService.DeleteUser(request);
    }
    
    public override async Task<UserListReply> GetUserByName(GetUserByNameRequest request, ServerCallContext context)
    {
        return await _userService.GetUserByName(request);
    }
    
    public override async Task<UserListReply> GetUserBySurname(GetUserBySurnameRequest request, ServerCallContext context)
    {
        return await _userService.GetUserBySurname(request);
    }
}