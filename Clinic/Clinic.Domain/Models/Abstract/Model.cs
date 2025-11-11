using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Domain.Models.Abstract;

/// <summary>
/// Defines a base model interface with a unique identifier.
/// </summary>
public abstract class Model
{
    /// <summary>
    /// Unique identifier of the data model.
    /// </summary>
    [Key] 
    [Column("id")]
    public virtual Guid Id { get; set; } = Guid.NewGuid();
}
