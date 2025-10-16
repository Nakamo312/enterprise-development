using Domain.Interfaces;

namespace Domain.Models;

/// <summary>
/// Represents an appointment in the medical clinic.
/// </summary>
public class Appointment : IModel
{
    /// <summary>
    /// unique identifier for the appointment.
    /// </summary>
    public required uint Id { get; set; }

    /// <summary>
    /// date and time of the appointment.
    /// </summary>
    public required DateTime DateTime { get; set; }

    /// <summary>
    /// room number where the appointment takes place.
    /// </summary>
    public required string RoomNumber { get; set; }

    /// <summary>
    /// value indicating whether the appointment is a repeated visit.
    /// </summary>
    public required bool IsRepeated { get; set; }

    /// <summary>
    /// patient associated with the appointment.
    /// </summary>
    public required uint PatientId { get; set; }

    /// <summary>
    /// doctor associated with the appointment.
    /// </summary>
    public required uint DoctorId { get; set; }
}
