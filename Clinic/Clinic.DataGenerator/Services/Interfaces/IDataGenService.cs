namespace Clinic.DataGenerator.Services.Interfaces;

/// <summary>
/// Interface for data generation services
/// </summary>
/// <typeparam name="T">Type of DTO to generate</typeparam>
public interface IDataGenService<T>
{
    /// <summary>
    /// Generates a single instance of T
    /// </summary>
    T Generate();

    /// <summary>
    /// Generates multiple instances of T
    /// </summary>
    /// <param name="count">Number of instances to generate</param>
    IEnumerable<T> Generate(int count);

    /// <summary>
    /// Gets the list of generated IDs for tracking purposes
    /// </summary>
    /// <returns>List of generated GUIDs</returns>
    List<Guid> GetGeneratedIds();
}