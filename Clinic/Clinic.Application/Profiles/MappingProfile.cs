using AutoMapper;
using Clinic.Domain.Models;
using Clinic.Application.DTOs;

namespace Clinic.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Appointment, AppointmentResponseDto>();
        CreateMap<Doctor, DoctorResponseDto>();
        CreateMap<Patient, PatientResponseDto>();
        CreateMap<Specialization, SpecializationResponseDto>();

        CreateMap<AppointmentCreateDto, Appointment>();
        CreateMap<AppointmentUpdateDto, Appointment>();
        CreateMap<DoctorCreateDto, Doctor>();
        CreateMap<DoctorUpdateDto, Doctor>();
        CreateMap<PatientCreateDto, Patient>();
        CreateMap<PatientUpdateDto, Patient>();
        CreateMap<SpecializationCreateDto, Specialization>();
        CreateMap<SpecializationUpdateDto, Specialization>();
    }
}
