using UserService.Database.Entities;

namespace UserService.Models;

public class User
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Age { get; set; }

    public User() { }

    public User(string login, string password, string name, string surname, int age)
    {
        Login = login;
        Password = password;
        Name = name;
        Surname = surname;
        Age = age;
    }
}
