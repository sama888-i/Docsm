using Docsm.DTOs.PatientDtos;
using FluentValidation;

namespace Docsm.Validators.PatientValidators
{
    public class PatientCreateDtoValidator:AbstractValidator<ProfileCreateOrUpdateDto>
    {
        public PatientCreateDtoValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Name can not be empty")
             .MaximumLength(32).WithMessage("Name length cannot be more than 32");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname can not be empty")
                .MaximumLength(32).WithMessage("Surname length can not be more than 32");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number  can not be empty")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits");

            RuleFor(x => x.DateOfBirth)
               .NotEmpty().WithMessage("Date of birth can not be empty")
               .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.")
               .Custom((date, context) =>
               {
                if (!DateTime.TryParse(date.ToString(), out _))
                {
                    context.AddFailure("Date of birth format is incorrect. Please use 'yyyy-MM-dd'.");
                }
               });
            RuleFor(x => x.Address)
                .MaximumLength(50).WithMessage("Address length can not be more than 50");
            RuleFor(x => x.Country)
                .MaximumLength(50).WithMessage("Country length can not be more than 50");
        }
    }
}
