using UserService.Models.DomainInterfaces;

namespace UserService.Repository;

public interface IUserRepository
{
    public Task<List<IUser>> GetAllAsync(CancellationToken cancellationToken);
    public Task<IUser?> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<List<IUser>> GetByNameAsync(string name, CancellationToken cancellationToken);
    public Task<List<IUser>> GetBySurnameAsync(string surname, CancellationToken cancellationToken);
    public Task<int> CreateUserAsync(IUser user, CancellationToken cancellationToken);
    public Task UpdateAsync(IUser user, CancellationToken cancellationToken);
    public Task DeleteAsync(int id, CancellationToken cancellationToken);
}