namespace UserService.Models.DomainInterfaces;

public interface IUserUpdateModel
{
    public int Id { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Age { get; set; }
}