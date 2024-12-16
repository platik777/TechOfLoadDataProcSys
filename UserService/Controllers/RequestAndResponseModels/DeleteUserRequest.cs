using UserService.Models;

namespace UserService;

public partial class DeleteUserRequest : IUser
{
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Age { get; set; }
}