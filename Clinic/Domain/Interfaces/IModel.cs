namespace Domain.Interfaces;

/// <summary>
/// Defines a base model interface with a unique identifier.
/// </summary>
public abstract class Model
{
    /// <summary>
    /// Unique identifier of the data model.
    /// </summary>
    public virtual uint Id { get; set; }
}
