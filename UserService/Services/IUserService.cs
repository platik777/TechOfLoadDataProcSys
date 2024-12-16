using UserService.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<List<IUser>> GetAllUsers(CancellationToken cancellationToken);
    Task<IUser> GetUserById(IUser request, CancellationToken cancellationToken);
    Task<List<IUser>> GetUserByName(IUser request, CancellationToken cancellationToken);
    Task<List<IUser>> GetUserBySurname(IUser request, CancellationToken cancellationToken);
    Task<IUser> CreateUser(IUser request, CancellationToken cancellationToken);
    Task<IUser> UpdateUser(IUser request, CancellationToken cancellationToken);
    Task<IUser> DeleteUser(IUser request, CancellationToken cancellationToken);
}