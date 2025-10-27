using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing appointment.
/// </summary>
public class AppointmentUpdateDto
{
    /// <summary>
    /// Date and time of the appointment.
    /// </summary>
    public DateTime? DateTime { get; set; }

    /// <summary>
    /// Room number where the appointment takes place.
    /// </summary>
    [StringLength(10, ErrorMessage = "Room number must not exceed 10 characters")]
    public string? RoomNumber { get; set; }

    /// <summary>
    /// Value indicating whether the appointment is a repeated visit.
    /// </summary>
    public bool? IsRepeated { get; set; }

    /// <summary>
    /// Patient associated with the appointment.
    /// </summary>
    public uint? PatientId { get; set; }

    /// <summary>
    /// Doctor associated with the appointment.
    /// </summary>
    public uint? DoctorId { get; set; }
}