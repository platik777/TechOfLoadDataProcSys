using Grpc.Core;
using UserService.Database.Entities;
using UserService.Models;

namespace UserService.Repository;

public interface IUserRepository
{
    public Task<List<UserEntity>> GetAllAsync(CancellationToken cancellationToken);
    public Task<UserEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<List<UserEntity>> GetByNameAsync(string name, CancellationToken cancellationToken);
    public Task<List<UserEntity>> GetBySurnameAsync(string surname, CancellationToken cancellationToken);
    public Task<int> CreateUserAsync(User user, CancellationToken cancellationToken);
    public Task UpdateAsync(User user, CancellationToken cancellationToken);
    public Task DeleteAsync(int id, CancellationToken cancellationToken);
}