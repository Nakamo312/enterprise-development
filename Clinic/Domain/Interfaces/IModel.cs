namespace Domain.Interfaces;

/// <summary>
/// Defines a base model interface with a unique identifier.
/// </summary>
public interface IModel
{
    /// <summary>
    /// unique identifier of the data model.
    /// </summary>
    public uint Id { get; set; }
}
