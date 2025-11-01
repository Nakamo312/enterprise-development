using Clinic.Domain.Abstract.Models;

namespace Clinic.Domain.Models;

/// <summary>
/// Represents a doctor in the medical clinic.
/// </summary>
public class Doctor : Model
{
    /// <summary>
    /// Passport number of the doctor.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Full name of the doctor.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Year of birth of the doctor.
    /// </summary>
    public required uint YearOfBirth { get; set; }

    /// <summary>
    /// Specialization of the doctor.
    /// </summary>
    public required uint SpecializationId { get; set; }

    /// <summary>
    /// Work experience (in years) of the doctor.
    /// </summary>
    public uint? Experience { get; set; }
}