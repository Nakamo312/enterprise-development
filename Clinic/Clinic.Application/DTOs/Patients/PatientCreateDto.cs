using Clinic.Domain.Enums;
using Clinic.Application.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Clinic.Application.Dtos.Patients;

/// <summary>
/// Data transfer object for creating a new patient.
/// </summary>
public class PatientCreateDto
{
    /// <summary>
    /// Passport number of the patient.
    /// </summary>
    [Required(ErrorMessage = "Passport number is required")]
    [RegularExpression(@"^\d{4}\s\d{6}$", ErrorMessage = "Passport number must be in format: XXXX XXXXXX")]
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the patient.
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
    public required string FullName { get; set; }

    /// <summary>
    /// Gender of the patient.
    /// </summary>
    [Required(ErrorMessage = "Gender is required")]
    [EnumRange(typeof(Gender))]
    public required Gender Gender { get; set; }

    /// <summary>
    /// Date of birth of the patient.
    /// </summary>
    [DefaultValue("1990-01-01")]
    [Required(ErrorMessage = "Date of birth is required")]
    public required DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Address of the patient.
    /// </summary>
    [Required(ErrorMessage = "Address is required")]
    [StringLength(200, ErrorMessage = "Address must not exceed 200 characters")]
    public required string Address { get; set; }

    /// <summary>
    /// Blood group of the patient.
    /// </summary>
    [Required(ErrorMessage = "Blood group is required")]
    [EnumRange(typeof(BloodGroup))]
    public required BloodGroup BloodGroup { get; set; }

    /// <summary>
    /// Rh factor of the patient.
    /// </summary>
    [Required(ErrorMessage = "Rh factor is required")]
    [EnumRange(typeof(RhFactor))]
    public required RhFactor RhFactor { get; set; }

    /// <summary>
    /// Contact phone number of the patient.
    /// </summary>
    [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Phone number must be in the format '+7 (XXX) XXX-XX-XX'")]
    [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters")]
    public string? ContactPhone { get; set; }
}