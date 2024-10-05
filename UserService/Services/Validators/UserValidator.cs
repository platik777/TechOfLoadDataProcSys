using FluentValidation;
using UserService.Models;

namespace UserService.Services.Validators;

public class UserValidator : AbstractValidator<User> 
{
    public UserValidator()
    {
        When(user => user.Name.HasValue)
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name shouldn't be empty").Length(2, 50)
            .WithMessage("Name should contains between 2 and 50 symbols");
        RuleFor(user => user.Surname).NotEmpty().WithMessage("Surname shouldn't be empty").Length(2, 50)
            .WithMessage("Surname should contains between 2 and 50 symbols");
        RuleFor(user => user.Login).NotEmpty().WithMessage("Login shouldn't be empty").Length(2, 50)
            .WithMessage("Login should contains between 2 and 50 symbols");
        RuleFor(user => user.Password).NotEmpty().WithMessage("Password shouldn't be empty").Length(2, 50)
            .WithMessage("Password should contains between 2 and 50 symbols");
        RuleFor(user => user.Age).NotEmpty().WithMessage("Age shouldn't be empty").InclusiveBetween(0, 120)
            .WithMessage("Age should be between 0 and 120");
    }
}