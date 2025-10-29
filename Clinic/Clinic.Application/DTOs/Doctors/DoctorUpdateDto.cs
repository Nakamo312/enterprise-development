using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.Dtos.Doctors;

/// <summary>
/// Data transfer object for updating an existing doctor.
/// </summary>
public class DoctorUpdateDto
{
    /// <summary>
    /// Passport number of the doctor.
    /// </summary>
    [RegularExpression(@"^\d{4}\s\d{6}$", ErrorMessage = "Passport number must be in format: XXXX XXXXXX")]
    public string? PassportNumber { get; set; }

    /// <summary>
    /// Full name of the doctor.
    /// </summary>
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
    public string? FullName { get; set; }

    /// <summary>
    /// Year of birth of the doctor.
    /// </summary>
    [DefaultValue(1990)]
    [Range(1900, 2100, ErrorMessage = "Year of birth must be between 1900 and 2100")]
    public uint? YearOfBirth { get; set; }

    /// <summary>
    /// Specialization of the doctor.
    /// </summary>
    public uint? SpecializationId { get; set; }

    /// <summary>
    /// Work experience (in years) of the doctor.
    /// </summary>
    [DefaultValue(0)]
    [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years")]
    public uint? Experience { get; set; }
}