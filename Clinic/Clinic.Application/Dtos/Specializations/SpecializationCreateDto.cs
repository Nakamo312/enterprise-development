using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.Dtos.Specializations;

/// <summary>
/// Data transfer object for creating a new specialization.
/// </summary>
public class SpecializationCreateDto
{
    /// <summary>
    /// Name of the specialization.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    public required string Name { get; set; }
}