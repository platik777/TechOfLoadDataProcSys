using UserService.Models;

namespace UserService.Services;

public interface IUserService
{
    public Task<List<User>> GetAllUsers(CancellationToken cancellationToken);
    public Task<User> GetUserById(GetUserByIdRequest request, CancellationToken cancellationToken);
    public Task<List<User>> GetUserByName(GetUserByNameRequest request, CancellationToken cancellationToken);
    public Task<List<User>> GetUserBySurname(GetUserBySurnameRequest request, CancellationToken cancellationToken);
    public Task<User> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
    public Task<User> UpdateUser(UpdateUserRequest request, CancellationToken cancellationToken);
    public Task<User> DeleteUser(DeleteUserRequest request, CancellationToken cancellationToken);
}