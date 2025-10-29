namespace Clinic.Application.DTOs.Analytics;

/// <summary>
/// Response DTO for queries returning names
/// </summary>
public class NamesResponseDto
{
    /// <summary>
    /// Collection of names
    /// </summary>
    public IEnumerable<string> Names { get; set; } = new List<string>();
}