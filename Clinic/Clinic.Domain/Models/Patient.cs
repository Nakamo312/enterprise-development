using Clinic.Domain.Interfaces;

using Clinic.Domain.Enums;

namespace Clinic.Domain.Models;

/// <summary>
/// Represents a patient in the medical clinic.
/// </summary>
public class Patient : IModel
{
    /// <summary>
    /// unique identifier for the patient.
    /// </summary>
    public required uint Id { get; set; }

    /// <summary>
    /// passport number of the patient.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// full name of the patient.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    ///  gender of the patient.
    /// </summary>
    public required Gender Gender { get; set; }

    /// <summary>
    /// date of birth of the patient.
    /// </summary>
    public required DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// address of the patient.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// blood group of the patient.
    /// </summary>
    public required BloodGroup BloodGroup { get; set; }

    /// <summary>
    /// Rh factor of the patient.
    /// </summary>
    public required RhFactor RhFactor { get; set; }

    /// <summary>
    /// contact phone number of the patient.
    /// </summary>
    public string? ContactPhone { get; set; }
}
