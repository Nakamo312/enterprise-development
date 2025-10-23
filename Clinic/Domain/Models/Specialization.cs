using Domain.Interfaces;

namespace Domain.Models;

/// <summary>
/// Represents a specialization of a doctor in the medical clinic.
/// </summary>
public class Specialization : Model
{
    /// <summary>
    /// Name of the specialization.
    /// </summary>
    public required string Name { get; set; }
}