using FluentValidation;
using Grpc.Core;
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

    public UserService(
        IUserRepository userRepository,
        IValidator<User> userCreateValidator,
        IValidator<User> userUpdateValidator)
    {
        _userRepository = userRepository;
        _userCreateValidator = userCreateValidator;
        _userUpdateValidator = userUpdateValidator;
    }

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
    {
        var userEntities = await _userRepository.GetAllAsync(cancellationToken);

        return userEntities.Select(userEntity => new User(userEntity)).ToList();
    }

    public async Task<User> GetUserById(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (userEntity == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        return new User(userEntity);
    }

    public async Task<List<User>> GetUserByName(GetUserByNameRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetByNameAsync(request.Name, cancellationToken);

        return users.Select(user => new User
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            })
            .ToList();
    }

    public async Task<List<User>> GetUserBySurname(GetUserBySurnameRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetBySurnameAsync(request.Surname, cancellationToken);

        return users.Select(user => new User
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            })
            .ToList();
    }

    public async Task<User> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Login = request.Login,
            Password = request.Password,
            Name = request.Name,
            Surname = request.Surname,
            Age = request.Age
        };

        var validationResult = _userCreateValidator.Validate(user);
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

        var existingUser = new User(existingUserEntity);

        var validationResult = _userUpdateValidator.Validate(existingUser);
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

        var user = new User(userEntity);

        await _userRepository.DeleteAsync(request.Id, cancellationToken);

        return user;
    }
}