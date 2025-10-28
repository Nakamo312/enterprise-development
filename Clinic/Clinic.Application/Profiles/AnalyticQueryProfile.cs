using AutoMapper;
using Clinic.Application.DTOs;

namespace Clinic.Application.Profiles;

/// <summary>
/// AutoMapper profile configuration for Analytic query DTOs.
/// Defines mappings for query parameters and response DTOs.
/// </summary>
public class AnalyticQueryProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the AnalyticQueryProfile class and configures query mappings.
    /// </summary>
    public AnalyticQueryProfile()
    {
        CreateMap<IdsResponseDto, IdsResponseDto>();
        CreateMap<NamesResponseDto, NamesResponseDto>();

        CreateMap<DoctorsExperienceQueryDto, DoctorsExperienceQueryDto>();
        CreateMap<DoctorPatientsQueryDto, DoctorPatientsQueryDto>();
        CreateMap<RepeatedAppointmentsQueryDto, RepeatedAppointmentsQueryDto>();
        CreateMap<RoomAppointmentsQueryDto, RoomAppointmentsQueryDto>();
    }
}