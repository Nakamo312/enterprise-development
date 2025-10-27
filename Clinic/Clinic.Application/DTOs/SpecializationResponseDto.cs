namespace Clinic.Application.DTOs;

/// <summary>
/// Data transfer object representing a specialization in response.
/// </summary>
public class SpecializationResponseDto
{
    /// <summary>
    /// Unique identifier of the data model.
    /// </summary>
    public required uint Id { get; set; }

    /// <summary>
    /// Name of the specialization.
    /// </summary>
    public required string Name { get; set; }
}