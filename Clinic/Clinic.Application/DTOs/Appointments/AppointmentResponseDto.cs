using Clinic.Domain.Models;

namespace Clinic.Application.DTOs.Appointments;

/// <summary>
/// Data transfer object representing an appointment in response.
/// </summary>
public class AppointmentResponseDto : Model
{
    /// <summary>
    /// Date and time of the appointment.
    /// </summary>
    public required DateTime DateTime { get; set; }

    /// <summary>
    /// Room number where the appointment takes place.
    /// </summary>
    public required string RoomNumber { get; set; }

    /// <summary>
    /// Value indicating whether the appointment is a repeated visit.
    /// </summary>
    public required bool IsRepeated { get; set; }

    /// <summary>
    /// Patient associated with the appointment.
    /// </summary>
    public required uint PatientId { get; set; }

    /// <summary>
    /// Doctor associated with the appointment.
    /// </summary>
    public required uint DoctorId { get; set; }
}