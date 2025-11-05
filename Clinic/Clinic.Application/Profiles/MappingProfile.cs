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
        CreateMap<PatientCreateDto, Patient>()
            .ForMember(dest => dest.ContactPhone,
                      opt => opt.MapFrom(src => NormalizePhone(src.ContactPhone)));
        CreateMap<SpecializationCreateDto, Specialization>();

        CreateMap<AppointmentUpdateDto, Appointment>();
        CreateMap<DoctorUpdateDto, Doctor>();
        CreateMap<PatientUpdateDto, Patient>()
            .ForMember(dest => dest.ContactPhone,
                      opt => opt.MapFrom(src => NormalizePhone(src.ContactPhone)));
        CreateMap<SpecializationUpdateDto, Specialization>();
    }

    private static string? NormalizePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return null;

        var digits = new string(phone.Where(char.IsDigit).ToArray());

        return "8" + digits[1..];
    }
}
