using Clinic.Domain.Enums;

namespace Clinic.Application.DTOs;

/// <summary>
/// Data transfer object representing a patient in response.
/// </summary>
public class PatientResponseDto
{
    /// <summary>
    /// Unique identifier of the data model.
    /// </summary>
    public required uint Id { get; set; }
    /// <summary>
    /// Passport number of the patient.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the patient.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Gender of the patient.
    /// </summary>
    public required Gender Gender { get; set; }

    /// <summary>
    /// Date of birth of the patient.
    /// </summary>
    public required DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Address of the patient.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Blood group of the patient.
    /// </summary>
    public required BloodGroup BloodGroup { get; set; }

    /// <summary>
    /// Rh factor of the patient.
    /// </summary>
    public required RhFactor RhFactor { get; set; }

    /// <summary>
    /// Contact phone number of the patient.
    /// </summary>
    public string? ContactPhone { get; set; }
}
