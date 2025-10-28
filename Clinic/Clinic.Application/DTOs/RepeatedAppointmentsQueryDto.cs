namespace Clinic.Application.DTOs;

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