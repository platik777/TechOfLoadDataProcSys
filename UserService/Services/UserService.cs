using FluentValidation;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;
using UserService.Models.DomainInterfaces;
using UserService.Repository;
using UserService.Services.Utils;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<IUser> _userCreateValidator;
    private readonly IValidator<IUser> _userUpdateValidator;
    private readonly IMemoryCache _memoryCache;

    public UserService(
        IUserRepository userRepository,
        IValidator<IUser> userCreateValidator,
        IValidator<IUser> userUpdateValidator,
        IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _userCreateValidator = userCreateValidator;
        _userUpdateValidator = userUpdateValidator;
        _memoryCache = memoryCache;
    }

    public async Task<List<IUser>> GetAllUsers(CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IUser> GetUserById(int id, CancellationToken cancellationToken)
    {

        _memoryCache.TryGetValue(id, out IUser? user);
        if (user == null)
        {
            user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user != null)
            {
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set(user.Id, user, options);
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {id} not found"));
            }
        }

        return user;
    }

    public async Task<List<IUser>> GetUserByName(string name, CancellationToken cancellationToken)
    {
        _memoryCache.TryGetValue(name, out List<IUser>? users);
        if (users == null)
        {
            users = await _userRepository.GetByNameAsync(name, cancellationToken);
            if (users.Count != 0)
            {
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set(name, users, options);
            }
        }

        return users;
    }

    public async Task<List<IUser>> GetUserBySurname(string surname, CancellationToken cancellationToken)
    {
        _memoryCache.TryGetValue(surname, out List<IUser>? users);
        if (users == null)
        {
            users = await _userRepository.GetBySurnameAsync(surname, cancellationToken);
            if (users.Count != 0)
            {
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set(surname, users, options);
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

    public async Task<IUser> UpdateUser(IUserUpdateModel request, CancellationToken cancellationToken)
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

    public async Task<IUser> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {id} not found"));
        }

        await _userRepository.DeleteAsync(id, cancellationToken);

        return user;
    }
}