using UserService.Models;

namespace UserService;

public partial class GetUserByNameRequest : IUser
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Surname { get; set; }
    public int Age { get; set; }
}