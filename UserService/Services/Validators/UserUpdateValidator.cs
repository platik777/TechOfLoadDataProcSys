using FluentValidation;
using UserService.Models;

namespace UserService.Services.Validators;

public class UserUpdateValidator : AbstractValidator<IUser>
{
    public UserUpdateValidator()
    {
        When(user => user.Name != null,() => RuleFor(user => user.Name)
            .Length(2, 50).WithMessage("Name should contains between 2 and 50 symbols"));
        When(user => user.Name != null,() => RuleFor(user => user.Surname)
            .Length(2, 50).WithMessage("Surname should contains between 2 and 50 symbols"));
        When(user => user.Name != null,() => RuleFor(user => user.Login)
            .Length(2, 50).WithMessage("Login should contains between 2 and 50 symbols"));
        When(user => user.Name != null,() => RuleFor(user => user.Password)
            .Length(2, 50).WithMessage("Password should contains between 2 and 50 symbols"));
        When(user => user.Name != null,() => RuleFor(user => user.Age)
            .InclusiveBetween(0, 120).WithMessage("Age should be between 0 and 120"));
    }
}