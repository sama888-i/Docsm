using Docsm.DTOs.AuthDtos;
using FluentValidation;

namespace Docsm.Validators.UserValidators
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UsernameOrEmail)
               .NotEmpty().WithMessage("Username or Email  can not be empty");
            RuleFor(x => x.Password)
              .NotEmpty().WithMessage("Password can not be empty")
              .MaximumLength(32).WithMessage("Password length cannot be more than 32")
              .MinimumLength(4).WithMessage("Password length cannot be less than 4");


        }
    }
}
