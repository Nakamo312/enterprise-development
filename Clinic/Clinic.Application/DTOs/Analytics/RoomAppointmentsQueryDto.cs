using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.Dtos.Analytics;

/// <summary>
/// DTO for room appointments query
/// </summary>
public class RoomAppointmentsQueryDto
{
    /// <summary>
    /// Room number
    /// </summary>
    [Required(ErrorMessage = "Room number is required")]
    [StringLength(10, ErrorMessage = "Room number must not exceed 10 characters")]
    public required string RoomNumber { get; set; } = string.Empty;

    /// <summary>
    /// Month start date
    /// </summary>
    [Required(ErrorMessage = "Date and time is required")]
    public required DateTime MonthStart { get; set; }

    /// <summary>
    /// Month end date
    /// </summary>
    [Required(ErrorMessage = "Date and time is required")]
    public required DateTime MonthEnd { get; set; }
}