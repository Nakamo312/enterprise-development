namespace Clinic.DataGenerator.Services.Interfaces;

/// <summary>
/// Provides methods for confirming entity creation and handling rollback scenarios
/// </summary>
public interface ICreationConfirmable
{
    /// <summary>
    /// Confirms successful entity creation and performs necessary cleanup
    /// </summary>
    /// <param name="dataHash">Unique hash identifying the entity data</param>
    void ConfirmCreation(string dataHash);

    /// <summary>
    /// Returns an entity to available state after failed creation attempt
    /// </summary>
    /// <param name="dataHash">Unique hash identifying the entity data</param>
    void ReturnToAvailable(string dataHash);
}