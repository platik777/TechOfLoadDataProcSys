using FluentValidation;
using Grpc.Core;
using UserService.Mapper;
using UserService.Models;
using UserService.Repository;
using UserService.Services.Utils;
using UserService.Services.Validators;

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
        var userEntities = await _userRepository.GetAllAsync(cancellationToken);

        return userEntities.Select(userEntity => _userEntityToUserMapper.MapToUser(userEntity)).ToList();
    }

    public async Task<User> GetUserById(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (userEntity == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        return _userEntityToUserMapper.MapToUser(userEntity);
    }

    public async Task<List<User>> GetUserByName(GetUserByNameRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetByNameAsync(request.Name, cancellationToken);

        return users.Select(user => _userEntityToUserMapper.MapToUser(user)).ToList();
    }

    public async Task<List<User>> GetUserBySurname(GetUserBySurnameRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetBySurnameAsync(request.Surname, cancellationToken);

        return users.Select(user => _userEntityToUserMapper.MapToUser(user)).ToList();
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

    /*{
        "login": "login",
        "password": "pass",
        "name": "dima",
        "surname": "borisov",
        "age": 20
    }*/

    public async Task<User> UpdateUser(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var existingUserEntity = _userRepository.GetByIdAsync(request.Id, cancellationToken).Result;

        existingUserEntity.Name = request.Name == "" ? existingUserEntity.Name : request.Name;
        existingUserEntity.Surname = request.Surname == "" ? existingUserEntity.Surname : request.Surname;
        existingUserEntity.Password = request.Password == "" ? existingUserEntity.Password : request.Password;
        existingUserEntity.Age = request.Age == 0 ? existingUserEntity.Age : request.Age;

        var existingUser = _userEntityToUserMapper.MapToUser(existingUserEntity);

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
        var userEntity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (userEntity == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        var user = _userEntityToUserMapper.MapToUser(userEntity);

        await _userRepository.DeleteAsync(request.Id, cancellationToken);

        return user;
    }
}