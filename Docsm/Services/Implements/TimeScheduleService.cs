using Docsm.DataAccess;
using Docsm.DTOs.DoctorScheduleDtos;
using Docsm.Exceptions;
using Docsm.Models;
using Docsm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Docsm.Services.Implements
{
    public class TimeScheduleService(ApoSystemDbContext _context) : ITimeScheduleService
    {
        public async Task CreateScheduleAsync(CreateScheduleDto dto)
        {   
            var schedule = new DoctorTimeSchedule
            { 
                DoctorId = dto.DoctorId,
                AppointmentDate =dto.AppointmentDate ,
                StartTime =dto.startTime,
                EndTime = dto.endTime
            };
            await  _context.DoctorTimeSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
            
        }

        public async Task DeleteScheduleAsync(int scheduleId)
        {
            var schedule = await _context.DoctorTimeSchedules.FindAsync(scheduleId);
            if (schedule == null)
                throw new NotFoundException<DoctorTimeSchedule>();
            if (!schedule.IsAvailable)
                throw new Exception("This time is already booked and cannot be deleted");
            _context.DoctorTimeSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetScheduleDto>> GetAllSchedulesAsync(int doctorId)
        {
            var schedule = await _context.DoctorTimeSchedules
                .Where(x => x.DoctorId == doctorId)
                .Select(x => new GetScheduleDto 
                { 
                    Id = x.Id,
                    AppointmentDate =x.AppointmentDate ,
                    startTime = x.StartTime,
                    endTime = x.EndTime,
                    IsAvailable=x.IsAvailable, 
                    DoctorId=x.DoctorId
                }).ToListAsync();
           

            return schedule;

        }

        public async Task UpdateScheduleAsync(int scheduleId, UpdateScheduleDto  dto)
        {
            var schedule = await _context.DoctorTimeSchedules.FindAsync(scheduleId);
            if (schedule == null)
                throw new NotFoundException<DoctorTimeSchedule>();
            if (!schedule.IsAvailable)
                throw new Exception("This time is already booked and cannot be changed");
            schedule.AppointmentDate = dto.AppointmentDate;
            schedule.StartTime = dto.startTime;
            schedule.EndTime = dto.endTime;
            await _context.SaveChangesAsync();
           
        }
    }
}
