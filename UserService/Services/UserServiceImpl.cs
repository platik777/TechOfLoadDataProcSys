using Grpc.Core;
using UserService.Models;
using UserService.Repository;
using UserService.Services.Validators;

namespace UserService.Services;
public class UserServiceImpl : UserService.UserServiceBase
{
   private readonly IUserRepository _userRepository;
   
   public UserServiceImpl(IUserRepository userRepository)
   { 
       _userRepository = userRepository;
   }

   // Получить всех пользователей
   public override async Task<GetAllUsersReply> GetAllUsers(GetAllUsersRequest request, ServerCallContext context)
   {
       var users = await _userRepository.GetAllAsync();
       
       var reply = new GetAllUsersReply();

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

   // Получить пользователя по ID
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

    // Создать пользователя
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

        var userCreateValidator = new UserCreateValidator();

        await userCreateValidator.ValidateAsync(user);

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
    
    // Обновить пользователя
    public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var existingUser = await GetUserById(new GetUserByIdRequest { Id = request.Id }, context);

        var updatedUser = new User
        {
            Login = existingUser.Login,
            Password = request.Password,
            Name = request.Name,
            Surname = request.Surname,
            Age = request.Age
        };
        
        var userUpdateValidator = new UserUpdateValidator();

        await userUpdateValidator.ValidateAsync(updatedUser);

        await _userRepository.UpdateAsync(updatedUser);

        return new UserReply
        {
            Id = updatedUser.Id,
            Login = updatedUser.Login,
            Password = updatedUser.Password,
            Name = updatedUser.Name,
            Surname = updatedUser.Surname,
            Age = updatedUser.Age
        };
    }
    
    // Удалить пользователя
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
}