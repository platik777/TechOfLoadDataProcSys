using Grpc.Core;
using UserService.Database.Entities;
using UserService.Models;
using UserService.Repository;
using UserService.Services.Utils;
using UserService.Services.Validators;

namespace UserService.Services;
public class UserService
{
   private readonly IUserRepository _userRepository;
   private readonly UserCreateValidator _userCreateValidator;
   private readonly UserUpdateValidator _userUpdateValidator;
   
   public UserService(
       IUserRepository userRepository,
       UserCreateValidator userCreateValidator,
       UserUpdateValidator userUpdateValidator)
   {
       _userRepository = userRepository;
       _userCreateValidator = userCreateValidator;
       _userUpdateValidator = userUpdateValidator;
   }

   public async Task<List<User>> GetAllUsers()
   {
       var userEntities = await _userRepository.GetAllAsync();

       return userEntities.Select(userEntity => new User(userEntity)).ToList();
   }

    public async Task<User> GetUserById(GetUserByIdRequest request)
    {
        var userEntity = await _userRepository.GetByIdAsync(request.Id);
        if (userEntity == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        return new User(userEntity);
    }

    public async Task<User> CreateUser(CreateUserRequest request)
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
        user.Id = await _userRepository.CreateUserAsync(user);

        return user;
    }
    
    /*{
        "login": "login",
        "password": "pass",
        "name": "dima",
        "surname": "borisov",
        "age": 20
    }*/
    
    public async Task<User> UpdateUser(UpdateUserRequest request)
    {
        var existingUserEntity = _userRepository.GetByIdAsync(request.Id).Result;
        
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

        existingUserEntity.Password = PasswordEncoder.HashPassword(request.Password);
        await _userRepository.UpdateAsync(existingUser);

        return existingUser;
    }
    
    public async Task<User> DeleteUser(DeleteUserRequest request)
    {
        var userEntity = await _userRepository.GetByIdAsync(request.Id);
        if (userEntity == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        var user = new User(userEntity);

        await _userRepository.DeleteAsync(request.Id);

        return user;
    }
    
    public async Task<List<User>> GetUserByName(GetUserByNameRequest request)
    {
        var users = await _userRepository.GetByNameAsync(request.Name);

        return users.Select(user => new User
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            })
            .ToList();
    }
    
    public async Task<List<User>> GetUserBySurname(GetUserBySurnameRequest request)
    {
        var users = await _userRepository.GetBySurnameAsync(request.Surname);

        return users.Select(user => new User
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            })
            .ToList();
    }
}