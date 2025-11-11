using Clinic.Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Domain.Models;

/// <summary>
/// Represents a specialization of a doctor in the medical clinic.
/// </summary>
[Table("specializations")]
public class Specialization : Model
{
    /// <summary>
    /// Name of the specialization.
    /// </summary>
    [Column("name")]
    public required string Name { get; set; }
}