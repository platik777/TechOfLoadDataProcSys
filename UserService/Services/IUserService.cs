using UserService.Models.DomainInterfaces;

namespace UserService.Services;

public interface IUserService
{
    Task<List<IUser>> GetAllUsers(CancellationToken cancellationToken);
    Task<IUser> GetUserById(int id, CancellationToken cancellationToken);
    Task<List<IUser>> GetUserByName(string name, CancellationToken cancellationToken);
    Task<List<IUser>> GetUserBySurname(string surname, CancellationToken cancellationToken);
    Task<IUser> CreateUser(IUser request, CancellationToken cancellationToken);
    Task<IUser> UpdateUser(IUserUpdateModel request, CancellationToken cancellationToken);
    Task<IUser> DeleteUser(int id, CancellationToken cancellationToken);
}