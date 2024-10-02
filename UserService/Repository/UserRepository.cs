using Dapper;
using UserService.Models;
using UserService.Services;

namespace UserService.Repository;

public class UserRepository : IUserRepository
{
    private readonly DbService _dbService;

    public UserRepository(DbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        using (var connection = _dbService.GetConnection())
        {
            var query = "SELECT * FROM GetUserById(@Id)";
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
        }
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using (var connection = _dbService.GetConnection())
        {
            var query = "SELECT * FROM getAllUsers()";
            return await connection.QueryAsync<User>(query);
        }
    }

    public async Task<int> CreateUserAsync(User user)
    {
        using (var connection = _dbService.GetConnection())
        {
            var query = "SELECT CreateUser(@Login, @Password, @Name, @Surname, @Age)";
            return await connection.ExecuteScalarAsync<int>(query, user);
        }
    }

    public async Task UpdateAsync(User user)
    {
        using (var connection = _dbService.GetConnection())
        {
            var query = "SELECT UpdateUser(@Id, @Login, @Password, @Name, @Surname, @Age)";
            await connection.ExecuteAsync(query, user);
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (var connection = _dbService.GetConnection())
        {
            var query = "SELECT DeleteUser(@Id)";
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
