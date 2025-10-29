using AutoMapper;
using Clinic.Application.DTOs.Analytics;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Services;

/// <summary>
/// Service for complex clinic queries and reports.
/// </summary>
public interface IAnalyticQueryService
{
    /// <summary>
    /// Gets doctors with experience greater than or equal to specified years.
    /// </summary>
    /// <param name="queryDto">Query parameters</param>
    /// <returns>List of doctor full names</returns>
    Task<NamesResponseDto> GetDoctorsWithExperienceAsync(DoctorsExperienceQueryDto queryDto);

    /// <summary>
    /// Gets patients by specific doctor.
    /// </summary>
    /// <param name="queryDto">Query parameters</param>
    /// <returns>List of patient full names</returns>
    Task<NamesResponseDto> GetPatientsByDoctorAsync(DoctorPatientsQueryDto queryDto);

    /// <summary>
    /// Gets repeated appointments within date range.
    /// </summary>
    /// <param name="queryDto">Query parameters</param>
    /// <returns>List of appointment IDs</returns>
    Task<IdsResponseDto> GetRepeatedAppointmentsAsync(RepeatedAppointmentsQueryDto queryDto);

    /// <summary>
    /// Gets patients older than 30 who visited multiple doctors.
    /// </summary>
    /// <returns>List of patient full names</returns>
    Task<NamesResponseDto> GetPatientsOver30WithMultipleDoctorsAsync();

    /// <summary>
    /// Gets appointments for current month in specified room.
    /// </summary>
    /// <param name="queryDto">Query parameters</param>
    /// <returns>List of appointment IDs</returns>
    Task<IdsResponseDto> GetAppointmentsForRoomThisMonthAsync(RoomAppointmentsQueryDto queryDto);
}
