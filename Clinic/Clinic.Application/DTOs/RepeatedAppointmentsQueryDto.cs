namespace Clinic.Application.DTOs;

/// <summary>
/// Data Transfer Object (DTO) for querying repeated appointments within a specified date range.
/// This DTO is used to encapsulate the start and end dates for the query.
/// </summary>
public class RepeatedAppointmentsQueryDto
{
    /// <summary>
    /// Start date of the range
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the range
    /// </summary>
    public DateTime EndDate { get; set; }
}