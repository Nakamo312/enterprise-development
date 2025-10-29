using Clinic.Domain.Models;

namespace Clinic.Application.DTOs.Specializations;

/// <summary>
/// Data transfer object representing a specialization in response.
/// </summary>
public class SpecializationResponseDto : Model
{
    /// <summary>
    /// Name of the specialization.
    /// </summary>
    public required string Name { get; set; }
}