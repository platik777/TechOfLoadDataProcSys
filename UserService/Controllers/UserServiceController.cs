﻿using Grpc.Core;
using UserService.Mapper;
using UserService.Services;

namespace UserService.Controllers;

public class UserServiceController : UserService.UserServiceBase
{
    private readonly IUserService _userService;
    private readonly IUserToUserReplyMapper _userToUserReplyMapper;

    public UserServiceController(IUserService userService, IUserToUserReplyMapper userToUserReplyMapper)
    {
        _userService = userService;
        _userToUserReplyMapper = userToUserReplyMapper;
    }
    
    public override async Task<UserListReply> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
    {
        var users =  await _userService.GetAllUsers(context.CancellationToken);
        var userListReply = new UserListReply();

        userListReply.Users.AddRange(users.Select(user => _userToUserReplyMapper.MapToUserReply(user)));

        return userListReply;
    }

    public override async Task<UserReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        var user = await _userService.GetUserById(request.Id, context.CancellationToken);
        return _userToUserReplyMapper.MapToUserReply(user);
    }

    public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var user = await _userService.CreateUser(request, context.CancellationToken);
        return _userToUserReplyMapper.MapToUserReply(user);
    }

    public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var user = await _userService.UpdateUser(request, context.CancellationToken);
        return _userToUserReplyMapper.MapToUserReply(user);
    }

    public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var user = await _userService.DeleteUser(request.Id, context.CancellationToken);
        return _userToUserReplyMapper.MapToUserReply(user);
    }

    public override async Task<UserListReply> GetUserByName(GetUserByNameRequest request, ServerCallContext context)
    {
        var users = await _userService.GetUserByName(request.Name, context.CancellationToken);
        var userListReply = new UserListReply();
        userListReply.Users.AddRange(users.Select(user => _userToUserReplyMapper.MapToUserReply(user)));
        return userListReply;
    }

    public override async Task<UserListReply> GetUserBySurname(GetUserBySurnameRequest request, ServerCallContext context)
    {
        var users = await _userService.GetUserBySurname(request.Surname, context.CancellationToken);
        var userListReply = new UserListReply();
        userListReply.Users.AddRange(users.Select(user => _userToUserReplyMapper.MapToUserReply(user)));
        return userListReply;
    }
}