using Clinic.Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Domain.Models;

/// <summary>
/// Represents an appointment in the medical clinic.
/// </summary>
[Table("appointments")]
public class Appointment : Model
{
    /// <summary>
    /// Date and time of the appointment.
    /// </summary>
    [Column("date_time")]
    public required DateTime DateTime { get; set; }

    /// <summary>
    /// Room number where the appointment takes place.
    /// </summary>
    [Column("room_number")]
    public required string RoomNumber { get; set; }

    /// <summary>
    /// Value indicating whether the appointment is a repeated visit.
    /// </summary>
    [Column("is_repeated")]
    public required bool IsRepeated { get; set; }

    /// <summary>
    /// Patient associated with the appointment.
    /// </summary>
    [Column("patient_id")]
    public required Guid PatientId { get; set; }

    /// <summary>
    /// Doctor associated with the appointment.
    /// </summary>
    [Column("doctor_id")]
    public required Guid DoctorId { get; set; }
}
