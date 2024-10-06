using Grpc.Core;
using UserService.Models;
using UserService.Repository;
using UserService.Services.Utils;
using UserService.Services.Validators;

namespace UserService.Services;
public class UserService : global::UserService.UserService.UserServiceBase
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

   public override async Task<UserListReply> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
   {
       var users = await _userRepository.GetAllAsync();
       
       var reply = new UserListReply();

       foreach (var user in users)
       {
           reply.Users.Add(new UserReply
           {
               Id = user.Id,
               Login = user.Login,
               Password = user.Password,
               Name = user.Name,
               Surname = user.Surname,
               Age = user.Age
           });
       }

       return reply;
   }

    public override async Task<UserReply> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        return new UserReply
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }

    public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
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
        var userId = await _userRepository.CreateUserAsync(user);  
        
        return new UserReply
        {
            Id = userId, 
            Login = user.Login,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }
    
    /*{
        "login": "login",
        "password": "pass",
        "name": "dima",
        "surname": "borisov",
        "age": 20
    }*/
    
    public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var existingUser = _userRepository.GetByIdAsync(request.Id).Result;
        
        existingUser.Name = request.Name == "" ? existingUser.Name : request.Name;
        existingUser.Surname = request.Surname == "" ? existingUser.Surname : request.Surname;
        existingUser.Password = request.Password == "" ? existingUser.Password : request.Password;
        existingUser.Age = request.Age == 0 ? existingUser.Age : request.Age;

        var validationResult = _userUpdateValidator.Validate(existingUser);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
        }

        existingUser.Password = PasswordEncoder.HashPassword(request.Password);
        await _userRepository.UpdateAsync(existingUser);

        return new UserReply
        {
            Id = existingUser.Id,
            Login = existingUser.Login,
            Password = existingUser.Password,
            Name = existingUser.Name,
            Surname = existingUser.Surname,
            Age = existingUser.Age
        };
    }
    
    public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        await _userRepository.DeleteAsync(request.Id);

        return new UserReply
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age
        };
    }
    
    public override async Task<UserListReply> GetUserByName(GetUserByNameRequest request, ServerCallContext context)
    {
        var users = await _userRepository.GetByNameAsync(request.Name);
    
        var userListReply = new UserListReply();
        foreach (var user in users)
        {
            userListReply.Users.Add(new UserReply
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            });
        }

        return userListReply;
    }
    
    public override async Task<UserListReply> GetUserBySurname(GetUserBySurnameRequest request, ServerCallContext context)
    {
        var users = await _userRepository.GetBySurnameAsync(request.Surname);
    
        var userListReply = new UserListReply();
        foreach (var user in users)
        {
            userListReply.Users.Add(new UserReply
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age
            });
        }

        return userListReply;
    }

}