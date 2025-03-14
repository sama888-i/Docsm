using Docsm.DTOs.AuthDtos;
using FluentValidation;

namespace Docsm.Validators.UserValidators
{
    public class RegisterDtoValidators:AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidators()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username can not be empty ")
                .MaximumLength(32).WithMessage("UserName length cannot be more than 32");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email can not be empty")
                .EmailAddress().WithMessage("Email is not in the correct format");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can not be empty")
                .MaximumLength(32).WithMessage("Name length cannot be more than 32");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname can not be empty")
                .MaximumLength(32).WithMessage("Surname length cannot be more than 32");

            RuleFor(x => x.Password)
              .NotEmpty().WithMessage("Password can not be empty")
              .MaximumLength(32).WithMessage("Password length cannot be more than 32")
              .MinimumLength(4).WithMessage("Password length cannot be less than 4");

            RuleFor(x => x.DateOfBirth)
           .NotEmpty().WithMessage("Date of birth cannot be empty")
           .Must(BeAtLeast18YearsOld).WithMessage("User must be at least 18 years old");
        }
        
        private bool BeAtLeast18YearsOld(DateTime   dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            return age >= 18;
        }

    }
    public class RegisterForDoctorValidators: AbstractValidator<DoctorRegisterDto>
    {
        public RegisterForDoctorValidators()
        {

            RuleFor(x => x.UserName)
               .NotEmpty().WithMessage("Username can not be empty ")
               .MaximumLength(32).WithMessage("UserName length cannot be more than 32");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email can not be empty")
                .EmailAddress().WithMessage("Email is not in the correct format");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can not be empty")
                .MaximumLength(32).WithMessage("Name length cannot be more than 32");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname can not be empty")
                .MaximumLength(32).WithMessage("Surname length cannot be more than 32");

            RuleFor(x => x.Password)
              .NotEmpty().WithMessage("Password can not be empty")
              .MaximumLength(32).WithMessage("Password length cannot be more than 32")
              .MinimumLength(4).WithMessage("Password length cannot be less than 4");

        }
    }


}
