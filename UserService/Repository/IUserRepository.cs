using UserService.Database.Entities;
using UserService.Models;

namespace UserService.Repository;

public interface IUserRepository
{
    public Task<UserEntity> GetByIdAsync(int id);
    public Task<IEnumerable<UserEntity>> GetAllAsync();
    public Task<int> CreateUserAsync(User user);
    public Task UpdateAsync(User user);
    public Task DeleteAsync(int id);
    public Task<List<UserEntity>> GetByNameAsync(string name);
    public Task<List<UserEntity>> GetBySurnameAsync(string surname);
}