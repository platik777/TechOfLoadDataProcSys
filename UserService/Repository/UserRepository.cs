using System.Collections;
using Dapper;
using Grpc.Core;
using Npgsql;
using UserService.Database.Entities;
using UserService.Mapper;
using UserService.Models;
using UserService.Services;

namespace UserService.Repository;

public class UserRepository : IUserRepository
{
    private readonly DbService _dbService;
    private readonly IUserEntityToUserMapper _userEntityToUserMapper;
    

    public UserRepository(DbService dbService, IUserEntityToUserMapper userEntityToUserMapper)
    {
        _dbService = dbService;
        _userEntityToUserMapper = userEntityToUserMapper;
    }
    
    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM getAllUsers()";
        
        using (var connection = _dbService.GetConnection())
        {
            var command = new CommandDefinition(query, cancellationToken: cancellationToken);
            var result = await connection.QueryAsync<UserEntity>(command);
            var userEntityList = result.ToList();
            return userEntityList.Select(t => _userEntityToUserMapper.MapToUser(t)).ToList();
        }
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
            
        var query = "SELECT * FROM GetUserById(@Id)";
        
        using (var connection = _dbService.GetConnection())
        {
            var command = new CommandDefinition(query, parameters: parameters, cancellationToken: cancellationToken);
            var result =  await connection.QueryFirstOrDefaultAsync<UserEntity>(command);
            if (result != null)
            {
                return _userEntityToUserMapper.MapToUser(result);
            }

            return null;
        }
    }
    
    public async Task<List<User>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Name", name);

        var query = "SELECT * FROM GetUserByName(@Name)";
        
        using (var connection = _dbService.GetConnection())
        {
            var command = new CommandDefinition(query, parameters: parameters, cancellationToken: cancellationToken);
            var result = await connection.QueryAsync<UserEntity>(command);
            var userEntityList = result.ToList();
            return userEntityList.Select(t => _userEntityToUserMapper.MapToUser(t)).ToList();
        }
    }

    public async Task<List<User>> GetBySurnameAsync(string surname, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Surname", surname);

        var query = "SELECT * FROM GetUserBySurname(@Surname)";
        
        using (var connection = _dbService.GetConnection())
        {
            var command = new CommandDefinition(query, parameters: parameters, cancellationToken: cancellationToken);
            var result = await connection.QueryAsync<UserEntity>(command);
            var userEntityList = result.ToList();
            return userEntityList.Select(t => _userEntityToUserMapper.MapToUser(t)).ToList();
        }
    }

    public async Task<int> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Login", user.Login);
        parameters.Add("Password", user.Password);
        parameters.Add("Name", user.Name);
        parameters.Add("Surname", user.Surname);
        parameters.Add("Age", user.Age);

        var query = "SELECT CreateUser(@Login, @Password, @Name, @Surname, @Age)";
        using (var connection = _dbService.GetConnection())
        {
            try
            {
                var command = new CommandDefinition(query, parameters: parameters, cancellationToken: cancellationToken);
                return await connection.ExecuteScalarAsync<int>(command);
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, $"User with login {user.Login} already exists"));
            }
        }
    }


    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", user.Id);
        parameters.Add("Password", user.Password);
        parameters.Add("Name", user.Name);
        parameters.Add("Surname", user.Surname);
        parameters.Add("Age", user.Age);

        var query = "SELECT UpdateUser(@Id, @Password, @Name, @Surname, @Age)";
        
        using (var connection = _dbService.GetConnection())
        {
            var command = new CommandDefinition(query, parameters: parameters, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(command);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var query = "SELECT DeleteUser(@Id)";
        
        using (var connection = _dbService.GetConnection())
        {
            var command = new CommandDefinition(query, parameters: parameters, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(command);
        }
    }
}
