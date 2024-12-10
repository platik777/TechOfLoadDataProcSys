using FluentValidation;
using Grpc.Core;
using UserService.Mapper;
using UserService.Models;
using UserService.Repository;
using UserService.Services.Utils;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<User> _userCreateValidator;
    private readonly IValidator<User> _userUpdateValidator;
    private readonly IUserEntityToUserMapper _userEntityToUserMapper;
    private readonly ICreateUserRequestToUserMapper _createUserRequestToUserMapper;

    public UserService(
        IUserRepository userRepository,
        IValidator<User> userCreateValidator,
        IValidator<User> userUpdateValidator,
        IUserEntityToUserMapper userEntityToUserMapper, 
        ICreateUserRequestToUserMapper createUserRequestToUserMapper)
    {
        _userRepository = userRepository;
        _userCreateValidator = userCreateValidator;
        _userUpdateValidator = userUpdateValidator;
        _userEntityToUserMapper = userEntityToUserMapper;
        _createUserRequestToUserMapper = createUserRequestToUserMapper;
    }

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task<User> GetUserById(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        return user;
    }

    public async Task<List<User>> GetUserByName(GetUserByNameRequest request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByNameAsync(request.Name, cancellationToken);
    }

    public async Task<List<User>> GetUserBySurname(GetUserBySurnameRequest request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetBySurnameAsync(request.Surname, cancellationToken);
    }

    public async Task<User> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = _createUserRequestToUserMapper.CreateUserRequestToUser(request);

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

    public async Task<User> UpdateUser(UpdateUserRequest request, CancellationToken cancellationToken)
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

    public async Task<User> DeleteUser(DeleteUserRequest request, CancellationToken cancellationToken)
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