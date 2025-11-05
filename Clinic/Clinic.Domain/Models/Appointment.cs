using Clinic.Domain.Models.Abstract;
using System;

namespace Clinic.Domain.Models;

/// <summary>
/// Represents an appointment in the medical clinic.
/// </summary>
public class Appointment : Model
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
    public required Guid PatientId { get; set; }

    /// <summary>
    /// Doctor associated with the appointment.
    /// </summary>
    public required Guid DoctorId { get; set; }
}
