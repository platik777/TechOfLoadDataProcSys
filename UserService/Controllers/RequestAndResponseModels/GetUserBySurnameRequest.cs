using UserService.Models;

namespace UserService;

public partial class GetUserBySurnameRequest : IUser
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}