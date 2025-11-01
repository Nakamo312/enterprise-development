using Clinic.Domain.Enums;
using Clinic.Application.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Clinic.Application.Dtos.Patients;

/// <summary>
/// Data transfer object for updating an existing patient.
/// </summary>
public class PatientUpdateDto
{
    /// <summary>
    /// Passport number of the patient.
    /// </summary>
    [RegularExpression(@"^\d{4}\s\d{6}$", ErrorMessage = "Passport number must be in format: XXXX XXXXXX")]
    public string? PassportNumber { get; set; }

    /// <summary>
    /// Full name of the patient.
    /// </summary>
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
    public string? FullName { get; set; }

    /// <summary>
    /// Gender of the patient.
    /// </summary>
    [EnumRange(typeof(Gender))]
    public Gender? Gender { get; set; }

    /// <summary>
    /// Date of birth of the patient.
    /// </summary>
    [DefaultValue("1990-01-01")]
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Address of the patient.
    /// </summary>
    [StringLength(200, ErrorMessage = "Address must not exceed 200 characters")]
    public string? Address { get; set; }

    /// <summary>
    /// Blood group of the patient.
    /// </summary>
    [EnumRange(typeof(BloodGroup))]
    public BloodGroup? BloodGroup { get; set; }

    /// <summary>
    /// Rh factor of the patient.
    /// </summary>
    [EnumRange(typeof(RhFactor))]
    public RhFactor? RhFactor { get; set; }

    /// <summary>
    /// Contact phone number of the patient.
    /// </summary>
    [RegularExpression(@"^(\+7|8)[\s\-]?(?:(?:\(\d{3}\)|\d{3}))[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$", ErrorMessage = "Phone number must be in the format '+7 (XXX) XXX-XX-XX'")]
    [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters")]
    public string? ContactPhone { get; set; }
}