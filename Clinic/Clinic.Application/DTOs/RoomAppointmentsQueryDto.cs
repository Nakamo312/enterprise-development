namespace Clinic.Application.DTOs;

/// <summary>
/// DTO for room appointments query
/// </summary>
public class RoomAppointmentsQueryDto
{
    /// <summary>
    /// Room number
    /// </summary>
    public string RoomNumber { get; set; } = string.Empty;

    /// <summary>
    /// Month start date
    /// </summary>
    public DateTime MonthStart { get; set; }

    /// <summary>
    /// Month end date
    /// </summary>
    public DateTime MonthEnd { get; set; }
}