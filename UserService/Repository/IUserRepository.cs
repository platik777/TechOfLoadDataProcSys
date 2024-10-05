using UserService.Models;

namespace UserService.Repository;

public interface IUserRepository
{
    public Task<User> GetByIdAsync(int id);
    public Task<IEnumerable<User>> GetAllAsync();
    public Task<int> CreateUserAsync(User user);
    public Task UpdateAsync(User user);
    public Task DeleteAsync(int id);
    public Task<IEnumerable<User>> GetByNameAsync(string name);
    public Task<IEnumerable<User>> GetBySurnameAsync(string surname);
}