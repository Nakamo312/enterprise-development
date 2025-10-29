using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.Dtos.Appointments;

/// <summary>
/// Data transfer object for creating a new appointment.
/// </summary>
public class AppointmentCreateDto
{
    /// <summary>
    /// Date and time of the appointment.
    /// </summary>
    [Required(ErrorMessage = "Date and time is required")]
    public required DateTime DateTime { get; set; }

    /// <summary>
    /// Room number where the appointment takes place.
    /// </summary>
    [Required(ErrorMessage = "Room number is required")]
    [StringLength(10, ErrorMessage = "Room number must not exceed 10 characters")]
    public required string RoomNumber { get; set; }

    /// <summary>
    /// Value indicating whether the appointment is a repeated visit.
    /// </summary>
    [Required(ErrorMessage = "IsRepeated flag is required")]
    public required bool IsRepeated { get; set; }

    /// <summary>
    /// Patient associated with the appointment.
    /// </summary>
    [Required(ErrorMessage = "Patient ID is required")]
    public required uint PatientId { get; set; }

    /// <summary>
    /// Doctor associated with the appointment.
    /// </summary>
    [Required(ErrorMessage = "Doctor ID is required")]
    public required uint DoctorId { get; set; }
}