using Clinic.Application.Dtos.Appointments;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;

namespace Clinic.Application.Services;

/// <summary>
/// Service for complex clinic queries and reports.
/// </summary>
public interface IAnalyticQueryService
{
    /// <summary>
    /// Retrieves doctors with work experience greater than or equal to the specified number of years.
    /// </summary>
    /// <param name="experience">Minimum years of experience required</param>
    /// <returns>Collection of doctors matching the experience criteria</returns>
    public Task<IEnumerable<DoctorResponseDto>> GetDoctorsWithExperienceAsync(uint experience);

    /// <summary>
    /// Retrieves all patients who have appointments with a specific doctor.
    /// </summary>
    /// <param name="doctorId">Unique identifier of the doctor</param>
    /// <returns>Collection of patients treated by the specified doctor</returns>
    public Task<IEnumerable<PatientResponseDto>> GetPatientsByDoctorAsync(Guid doctorId);

    /// <summary>
    /// Retrieves repeated appointments within the specified date range.
    /// </summary>
    /// <param name="startDate">Start date of the search range</param>
    /// <param name="endDate">End date of the search range</param>
    /// <returns>Collection of appointment IDs for repeated appointments in the date range</returns>
    public Task<IEnumerable<AppointmentResponseDto>> GetRepeatedAppointmentsAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Retrieves patients over 30 years old who have been treated by multiple different doctors.
    /// </summary>
    /// <returns>Collection of patients meeting the age and multiple doctors criteria</returns>
    public Task<IEnumerable<PatientResponseDto>> GetPatientsOver30WithMultipleDoctorsAsync();

    /// <summary>
    /// Retrieves appointments for a specific room during the current month.
    /// </summary>
    /// <param name="roomNumber">Query parameters containing room number and date range</param>
    /// <returns>Collection of appointment IDs for the specified room in current month</returns>
    public Task<IEnumerable<AppointmentResponseDto>> GetAppointmentsForRoomThisMonthAsync(string roomNumber);
}
