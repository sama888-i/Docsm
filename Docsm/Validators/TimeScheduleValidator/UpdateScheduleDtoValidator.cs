using Docsm.DTOs.DoctorScheduleDtos;
using FluentValidation;

namespace Docsm.Validators.TimeScheduleValidator
{
    public class UpdateScheduleDtoValidator:AbstractValidator<UpdateScheduleDto>
    {
        public UpdateScheduleDtoValidator()
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
