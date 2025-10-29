using AutoMapper;
using Clinic.Domain.Models;
using Clinic.Application.Dtos.Appointments;
using Clinic.Application.Dtos.Doctors;
using Clinic.Application.Dtos.Patients;
using Clinic.Application.Dtos.Specializations;

namespace Clinic.Application.Profiles;

/// <summary>
/// AutoMapper profile configuration for mapping between domain models and DTOs.
/// Defines all object-to-object mappings used in the application.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the MappingProfile class and configures all mappings.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Appointment, AppointmentResponseDto>();
        CreateMap<Doctor, DoctorResponseDto>();
        CreateMap<Patient, PatientResponseDto>();
        CreateMap<Specialization, SpecializationResponseDto>();

        CreateMap<AppointmentCreateDto, Appointment>();
        CreateMap<DoctorCreateDto, Doctor>();
        CreateMap<PatientCreateDto, Patient>();
        CreateMap<SpecializationCreateDto, Specialization>();

        CreateMap<AppointmentUpdateDto, Appointment>();
        CreateMap<DoctorUpdateDto, Doctor>();
        CreateMap<PatientUpdateDto, Patient>();
        CreateMap<SpecializationUpdateDto, Specialization>();
    }
}
