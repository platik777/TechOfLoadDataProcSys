using Grpc.Core;
using UserService.Models;

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
        var users =  await _userService.GetAllUsers();
        var userListReply = new UserListReply();
        foreach (var user in users)
        {
            userListReply.Users.Add(new UserReply()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            });
        }

        return userListReply;
    }

    public override async Task<UserReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        var user = await _userService.GetUserById(request);
        return new UserReply()
        {
            Id = user.Id,
            Login = user.Login,
            Password = user.Password,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }

    public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var user =  await _userService.CreateUser(request);
        return new UserReply()
        {
            Id = user.Id,
            Login = user.Login,
            Password = user.Password,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }
    
    public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var user = await _userService.UpdateUser(request);
        return new UserReply()
        {
            Id = user.Id,
            Login = user.Login,
            Password = user.Password,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }
    
    public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var user = await _userService.DeleteUser(request);
        return new UserReply()
        {
            Id = user.Id,
            Login = user.Login,
            Password = user.Password,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }
    
    public override async Task<UserListReply> GetUserByName(GetUserByNameRequest request, ServerCallContext context)
    {
        var users = await _userService.GetUserByName(request);
        var userListReply = new UserListReply();
        foreach (var user in users)
        {
            userListReply.Users.Add(new UserReply()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            });
        }

        return userListReply;
    }
    
    public override async Task<UserListReply> GetUserBySurname(GetUserBySurnameRequest request, ServerCallContext context)
    {
        var users = await _userService.GetUserBySurname(request);
        var userListReply = new UserListReply();
        foreach (var user in users)
        {
            userListReply.Users.Add(new UserReply()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            });
        }

        return userListReply;
    }
}