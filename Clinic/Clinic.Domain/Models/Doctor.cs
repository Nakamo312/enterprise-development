using Clinic.Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Domain.Models;

/// <summary>
/// Represents a doctor in the medical clinic.
/// </summary>
[Table("doctors")]
public class Doctor : Model
{
    /// <summary>
    /// Passport number of the doctor.
    /// </summary>
    [Column("passport_number")]
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the doctor.
    /// </summary>
    [Column("full_name")]
    public required string FullName { get; set; }

    /// <summary>
    /// Year of birth of the doctor.
    /// </summary>
    [Column("year_of_birth")]
    public required uint YearOfBirth { get; set; }

    /// <summary>
    /// Specialization of the doctor.
    /// </summary>
    [Column("specialization_id")]
    public required Guid SpecializationId { get; set; }

    /// <summary>
    /// Work experience (in years) of the doctor.
    /// </summary>
    [Column("experience")]
    public uint? Experience { get; set; }
}