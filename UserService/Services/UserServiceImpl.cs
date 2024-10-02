using Grpc.Core;
using UserService.Models;
using UserService.Repository;

namespace UserService.Services;
public class UserServiceImpl : UserService.UserServiceBase
{
   private readonly IUserRepository _userRepository;
   
   public UserServiceImpl(IUserRepository userRepository)
   { 
       _userRepository = userRepository;
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