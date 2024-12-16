using UserService.Models;

namespace UserService
{
    public partial class CreateUserRequest : IUser
    {
        public int Id { get; set; }
    }
}