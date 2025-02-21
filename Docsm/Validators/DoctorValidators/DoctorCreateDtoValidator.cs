using Docsm.DTOs.DoctorDtos;
using FluentValidation;

namespace Docsm.Validators.DoctorValidators
{
    public class DoctorCreateDtoValidator:AbstractValidator<DoctorCreateDto>
    {
        public DoctorCreateDtoValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Name can not be empty")
              .MaximumLength(32).WithMessage("Name length cannot be more than 32");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname can not be empty")
                .MaximumLength(32).WithMessage("Surname length cannot be more than 32");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number  can not be empty")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits");
            RuleFor(x => x.DateOfBirth)
               .NotEmpty().WithMessage("Date of birth can not be empty")
               .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

        }
    }
}
