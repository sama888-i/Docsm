using Docsm.DTOs.AuthDtos;
using FluentValidation;

namespace Docsm.Validators.UserValidators
{
    public class ForgotPasswordDtoValidator:AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email can not be empty")
                .EmailAddress().WithMessage("Email is not in the correct format");
        }

    }
    public class ResetPasswordDtoValidator: AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email can not be empty")
                .EmailAddress().WithMessage("Email is not in the correct format");
            RuleFor(x => x.NewPassword)
             .NotEmpty().WithMessage("Password can not be empty")
             .MaximumLength(32).WithMessage("Password length cannot be more than 32")
             .MinimumLength(4).WithMessage("Password length cannot be less than 4");

        }
    }

}
