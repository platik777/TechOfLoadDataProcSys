using Dapper;
using UserService.Database.Entities;
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

    public async Task<UserEntity> GetByIdAsync(int id)
    {
        using (var connection = _dbService.GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            
            var query = "SELECT * FROM GetUserById(@Id)";
            return await connection.QueryFirstOrDefaultAsync<UserEntity>(query, parameters);
        }
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        using (var connection = _dbService.GetConnection())
        {
            var query = "SELECT * FROM getAllUsers()";
            return await connection.QueryAsync<UserEntity>(query);
        }
    }

    public async Task<int> CreateUserAsync(User user)
    {
        using (var connection = _dbService.GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("Login", user.Login);
            parameters.Add("Password", user.Password);
            parameters.Add("Name", user.Name);
            parameters.Add("Surname", user.Surname);
            parameters.Add("Age", user.Age);

            var query = "SELECT CreateUser(@Login, @Password, @Name, @Surname, @Age)";
            return await connection.ExecuteScalarAsync<int>(query, parameters);
        }
    }

    public async Task UpdateAsync(User user)
    {
        using (var connection = _dbService.GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", user.Id);
            parameters.Add("Password", user.Password);
            parameters.Add("Name", user.Name);
            parameters.Add("Surname", user.Surname);
            parameters.Add("Age", user.Age);

            var query = "SELECT UpdateUser(@Id, @Password, @Name, @Surname, @Age)";
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (var connection = _dbService.GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var query = "SELECT DeleteUser(@Id)";
            await connection.ExecuteAsync(query, parameters);
        }
    }

    public async Task<List<UserEntity>> GetByNameAsync(string name)
    {
        using (var connection = _dbService.GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", name);

            var query = "SELECT * FROM GetUserByName(@Name)";
            var result = await connection.QueryAsync<UserEntity>(query, parameters);
            return result.ToList(); 
        }
    }

    public async Task<List<UserEntity>> GetBySurnameAsync(string surname)
    {
        using (var connection = _dbService.GetConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("Surname", surname);

            var query = "SELECT * FROM GetUserBySurname(@Surname)";
            var result = await connection.QueryAsync<UserEntity>(query, parameters);
            return result.ToList();
        }
    }
}
