using Clinic.Domain.Models.Abstract;
using Clinic.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Domain.Models;

/// <summary>
/// Represents a patient in the medical clinic.
/// </summary>
[Table("patients")]
public class Patient : Model
{
    /// <summary>
    /// Passport number of the patient.
    /// </summary>
    [Column("passport_number")]
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the patient.
    /// </summary>
    [Column("full_name")]
    public required string FullName { get; set; }

    /// <summary>
    /// Gender of the patient.
    /// </summary>
    [Column("gender")]
    public required Gender Gender { get; set; }

    /// <summary>
    /// Date of birth of the patient.
    /// </summary>
    [Column("date_of_birth")]
    public required DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Address of the patient.
    /// </summary>
    [Column("address")]
    public required string Address { get; set; }

    /// <summary>
    /// Blood group of the patient.
    /// </summary>
    [Column("blood_group")]
    public required BloodGroup BloodGroup { get; set; }

    /// <summary>
    /// Rh factor of the patient.
    /// </summary>
    [Column("rh_factor")]
    public required RhFactor RhFactor { get; set; }

    /// <summary>
    /// Contact phone number of the patient.
    /// </summary>
    [Column("contact_phone")]
    public string? ContactPhone { get; set; }
}
