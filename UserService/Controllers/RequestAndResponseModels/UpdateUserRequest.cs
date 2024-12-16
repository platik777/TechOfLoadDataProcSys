using UserService.Models;

namespace UserService;

public partial class UpdateUserRequest : IUser
{
    public string? Login { get; set; }
}