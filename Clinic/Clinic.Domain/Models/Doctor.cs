using Clinic.Domain.Interfaces;

namespace Clinic.Domain.Models;

/// <summary>
/// Represents a doctor in the medical clinic.
/// </summary>
public class Doctor : IModel
{
    /// <summary>
    /// unique identifier for the doctor.
    /// </summary>
    public required uint Id { get; set; }

    /// <summary>
    /// passport number of the doctor.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// full name of the doctor.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// year of birth of the doctor.
    /// </summary>
    public required uint YearOfBirth { get; set; }

    /// <summary>
    /// specialization of the doctor.
    /// </summary>
    public required uint SpecializationId { get; set; }

    /// <summary>
    /// work experience (in years) of the doctor.
    /// </summary>
    public uint? Experience { get; set; }
}