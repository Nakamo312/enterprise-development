namespace Clinic.Application.DTOs;

/// <summary>
/// Data transfer object representing a patient in response.
/// </summary>
public class DoctorResponseDto
{
    /// <summary>
    /// Unique identifier of the data model.
    /// </summary>
    public required uint Id { get; set; }
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
