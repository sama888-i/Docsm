using Docsm.DTOs.DoctorScheduleDtos;
using FluentValidation;

namespace Docsm.Validators.TimeScheduleValidator
{
    public class CreateScheduleDtoValidator:AbstractValidator<CreateScheduleDto>
    {
        public CreateScheduleDtoValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage("AppointmentDate can not be empty");
            RuleFor(x => x.startTime)
                .NotEmpty().WithMessage("Start time can not be empty");
            RuleFor(x => x.endTime)
                .NotEmpty().WithMessage("End time can not be empty");

        }
    }
}
