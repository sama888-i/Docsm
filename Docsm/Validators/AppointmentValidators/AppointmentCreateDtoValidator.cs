using Docsm.DTOs.AppointmentDtos;
using FluentValidation;

namespace Docsm.Validators.AppointmentValidators
{
    public class AppointmentCreateDtoValidator:AbstractValidator<AppointmentCreateDto>
    {
        public AppointmentCreateDtoValidator()
        {
            RuleFor(x => x.ReasonAppointment)
              .MaximumLength(100).WithMessage("Reason length must be less than 100");
        }
    }
}
