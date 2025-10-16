using Domain.Interfaces;

namespace Domain.Models;

/// <summary>
/// Represents a specialization of a doctor in the medical clinic.
/// </summary>
public class Specialization : IModel
{
    /// <summary>
    /// unique identifier for the specialization.
    /// </summary>
    public required uint Id { get; set; }

    /// <summary>
    /// name of the specialization.
    /// </summary>
    public required string Name { get; set; }
}