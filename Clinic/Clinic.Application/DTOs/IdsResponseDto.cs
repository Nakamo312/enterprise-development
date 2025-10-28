namespace Clinic.Application.DTOs;

/// <summary>
/// Response DTO for queries returning IDs
/// </summary>
public class IdsResponseDto
{
    /// <summary>
    /// Collection of IDs
    /// </summary>
    public IEnumerable<uint> Ids { get; set; } = new List<uint>();
}