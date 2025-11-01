using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.Dtos.Specializations;

/// <summary>
/// Data transfer object for updating an existing specialization.
/// </summary>
public class SpecializationUpdateDto
{

    /// <summary>
    /// Name of the specialization.
    /// </summary>
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    public string? Name { get; set; }
}