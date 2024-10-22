using UserService.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken);
    Task<User> GetUserById(GetUserByIdRequest request, CancellationToken cancellationToken);
    Task<List<User>> GetUserByName(GetUserByNameRequest request, CancellationToken cancellationToken);
    Task<List<User>> GetUserBySurname(GetUserBySurnameRequest request, CancellationToken cancellationToken);
    Task<User> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
    Task<User> UpdateUser(UpdateUserRequest request, CancellationToken cancellationToken);
    Task<User> DeleteUser(DeleteUserRequest request, CancellationToken cancellationToken);
}