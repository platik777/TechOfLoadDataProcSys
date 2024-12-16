using FluentValidation;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;
using UserService.Mapper;
using UserService.Models;
using UserService.Repository;
using UserService.Services.Utils;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<IUser> _userCreateValidator;
    private readonly IValidator<IUser> _userUpdateValidator;
    private readonly IUserEntityToUserMapper _userEntityToUserMapper;
    private readonly ICreateUserRequestToUserMapper _createUserRequestToUserMapper;
    private readonly IMemoryCache _memoryCache;

    public UserService(
        IUserRepository userRepository,
        IValidator<IUser> userCreateValidator,
        IValidator<IUser> userUpdateValidator,
        IUserEntityToUserMapper userEntityToUserMapper, 
        ICreateUserRequestToUserMapper createUserRequestToUserMapper,
        IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _userCreateValidator = userCreateValidator;
        _userUpdateValidator = userUpdateValidator;
        _userEntityToUserMapper = userEntityToUserMapper;
        _createUserRequestToUserMapper = createUserRequestToUserMapper;
        _memoryCache = memoryCache;
    }

    public async Task<List<IUser>> GetAllUsers(CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IUser> GetUserById(IUser request, CancellationToken cancellationToken)
    {

        _memoryCache.TryGetValue(request.Id, out IUser? user);
        if (user == null)
        {
            user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user != null)
            {
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set(user.Id, user, options);
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
            }
        }

        return user;
    }

    public async Task<List<IUser>> GetUserByName(IUser request, CancellationToken cancellationToken)
    {
        _memoryCache.TryGetValue(request.Name, out List<IUser>? users);
        if (users == null)
        {
            users = await _userRepository.GetByNameAsync(request.Name, cancellationToken);
            if (users.Count != 0)
            {
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set(request.Name, users, options);
            }
        }

        return users;
    }

    public async Task<List<IUser>> GetUserBySurname(IUser request, CancellationToken cancellationToken)
    {
        _memoryCache.TryGetValue(request.Surname, out List<IUser>? users);
        if (users == null)
        {
            users = await _userRepository.GetBySurnameAsync(request.Surname, cancellationToken);
            if (users.Count != 0)
            {
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set(request.Surname, users, options);
            }
        }

        return users;
    }

    public async Task<IUser> CreateUser(IUser request, CancellationToken cancellationToken)
    {
        var user = request;

        var validationResult = await _userCreateValidator.ValidateAsync(user, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
        }

        user.Password = PasswordEncoder.HashPassword(request.Password);
        user.Id = await _userRepository.CreateUserAsync(user, cancellationToken);

        return user;
    }

    public async Task<IUser> UpdateUser(IUser request, CancellationToken cancellationToken)
    {
        var existingUser = _userRepository.GetByIdAsync(request.Id, cancellationToken).Result;

        if (existingUser == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }
        existingUser.Name = request.Name == "" ? existingUser.Name : request.Name;
        existingUser.Surname = request.Surname == "" ? existingUser.Surname : request.Surname;
        existingUser.Password = request.Password == "" ? existingUser.Password : request.Password;
        existingUser.Age = request.Age == 0 ? existingUser.Age : request.Age;

        var validationResult = await _userUpdateValidator.ValidateAsync(existingUser, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
        }

        existingUser.Password = PasswordEncoder.HashPassword(request.Password);
        await _userRepository.UpdateAsync(existingUser, cancellationToken);

        return existingUser;
    }

    public async Task<IUser> DeleteUser(IUser request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        await _userRepository.DeleteAsync(request.Id, cancellationToken);

        return user;
    }
}