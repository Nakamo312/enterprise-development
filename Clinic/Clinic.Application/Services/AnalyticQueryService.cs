using AutoMapper;
using Clinic.Application.Services;
using Clinic.Domain.Models;

using Clinic.Infrastructure.Repositories;
using Clinic.Application.DTOs.Analytics;

/// <summary>
/// Implementation of clinic query service.
/// </summary>
public class AnalyticQueryService
    (
        IRepository<Doctor> doctorRepository,
        IRepository<Patient> patientRepository,
        IRepository<Appointment> appointmentRepository,
        IMapper mapper
    ) : IAnalyticQueryService
{
    private readonly IRepository<Doctor> _doctorRepository = doctorRepository;
    private readonly IRepository<Patient> _patientRepository = patientRepository;
    private readonly IRepository<Appointment> _appointmentRepository = appointmentRepository;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<NamesResponseDto> GetDoctorsWithExperienceAsync(DoctorsExperienceQueryDto queryDto)
    {
        try
        {
            var doctors = await _doctorRepository.GetAsync();
            var names = doctors
                .Where(d => d.Experience >= queryDto.MinExperience)
                .Select(d => d.FullName)
                .Order()
                .ToList();

            return new NamesResponseDto { Names = names };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve doctors by experience", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<NamesResponseDto> GetPatientsByDoctorAsync(DoctorPatientsQueryDto queryDto)
    {
        try
        {
            var appointments = await _appointmentRepository.GetAsync();
            var patients = await _patientRepository.GetAsync();

            var patientFullNames = appointments
                .Where(a => a.DoctorId == queryDto.DoctorId)
                .Join(
                    patients,
                    a => a.PatientId,
                    p => p.Id,
                    (a, p) => p
                )
                .DistinctBy(p => p.Id)
                .OrderBy(p => p.FullName)
                .Select(p => p.FullName)
                .ToList();

            return new NamesResponseDto { Names = patientFullNames };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve patients for doctor with ID {queryDto.DoctorId}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IdsResponseDto> GetRepeatedAppointmentsAsync(RepeatedAppointmentsQueryDto queryDto)
    {
        try
        {
            var appointments = await _appointmentRepository.GetAsync();
            var ids = appointments
                .Where(a => a.IsRepeated && a.DateTime >= queryDto.StartDate && a.DateTime <= queryDto.EndDate)
                .Select(a => a.Id)
                .ToList();

            return new IdsResponseDto { Ids = ids };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve repeated appointments", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<NamesResponseDto> GetPatientsOver30WithMultipleDoctorsAsync()
    {
        try
        {
            var currentDate = DateTime.Now;
            var appointments = await _appointmentRepository.GetAsync();
            var patients = await _patientRepository.GetAsync();

            var patientFullNames = appointments
                .GroupBy(a => a.PatientId)
                .Where(g => g.Select(a => a.DoctorId).Distinct().Count() > 1)
                .Select(g => patients.First(p => p.Id == g.Key))
                .Where(p => p.DateOfBirth < DateOnly.FromDateTime(currentDate).AddYears(-30))
                .OrderBy(p => p.FullName)
                .Select(p => p.FullName)
                .ToList();

            return new NamesResponseDto { Names = patientFullNames };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve patients over 30 with multiple doctors", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IdsResponseDto> GetAppointmentsForRoomThisMonthAsync(RoomAppointmentsQueryDto queryDto)
    {
        try
        {
            var appointments = await _appointmentRepository.GetAsync();
            var ids = appointments
                .Where(a => a.DateTime.Date >= queryDto.MonthStart && a.DateTime.Date <= queryDto.MonthEnd && a.RoomNumber == queryDto.RoomNumber)
                .Select(a => a.Id)
                .ToList();

            return new IdsResponseDto { Ids = ids };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve appointments for room {queryDto.RoomNumber}", ex);
        }
    }
}