using AutoMapper;
using Clinic.Application.Services;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories;
using Clinic.Application.Dtos.Analytics;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;

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
    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorResponseDto>> GetDoctorsWithExperienceAsync(uint experience)
    {
        try
        {
            var doctors = await doctorRepository.GetAsync();
            var filteredDoctors = doctors
                .Where(d => d.Experience >= experience)
                .OrderBy(d => d.FullName)
                .ToList();

            return mapper.Map<List<DoctorResponseDto>>(filteredDoctors);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve doctors by experience", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PatientResponseDto>> GetPatientsByDoctorAsync(uint doctorId)
    {
        try
        {
            var appointments = await appointmentRepository.GetAsync();
            var patients = await patientRepository.GetAsync();

            var filteredPatients = appointments
                .Where(a => a.DoctorId == doctorId)
                .Join(
                    patients,
                    a => a.PatientId,
                    p => p.Id,
                    (a, p) => p
                )
                .DistinctBy(p => p.Id)
                .OrderBy(p => p.FullName)
                .ToList();

            return mapper.Map<List<PatientResponseDto>>(filteredPatients);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve patients for doctor with ID {doctorId}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<uint>> GetRepeatedAppointmentsAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var appointments = await appointmentRepository.GetAsync();
            var ids = appointments
                .Where(a => a.IsRepeated && a.DateTime >= startDate && a.DateTime <= endDate)
                .Select(a => a.Id)
                .ToList();

            return ids;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve repeated appointments", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PatientResponseDto>> GetPatientsOver30WithMultipleDoctorsAsync()
    {
        try
        {
            var currentDate = DateTime.Now;
            var appointments = await appointmentRepository.GetAsync();
            var patients = await patientRepository.GetAsync();

            var filteredPatients = appointments
                .GroupBy(a => a.PatientId)
                .Where(g => g.Select(a => a.DoctorId).Distinct().Count() > 1)
                .Select(g => patients.First(p => p.Id == g.Key))
                .Where(p => p.DateOfBirth < DateOnly.FromDateTime(currentDate).AddYears(-30))
                .OrderBy(p => p.FullName)
                .ToList();

            return mapper.Map<List<PatientResponseDto>>(filteredPatients);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve patients over 30 with multiple doctors", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<uint>> GetAppointmentsForRoomThisMonthAsync(RoomAppointmentsQueryDto queryDto)
    {
        try
        {
            var appointments = await appointmentRepository.GetAsync();
            var ids = appointments
                .Where(a => a.DateTime.Date >= queryDto.MonthStart && a.DateTime.Date <= queryDto.MonthEnd && a.RoomNumber == queryDto.RoomNumber)
                .Select(a => a.Id)
                .ToList();

            return ids;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve appointments for room {queryDto.RoomNumber}", ex);
        }
    }
}