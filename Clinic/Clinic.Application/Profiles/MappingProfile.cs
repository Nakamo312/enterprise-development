using AutoMapper;
using Clinic.Domain.Models;
using Clinic.Application.DTOs;

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

        CreateMap<AppointmentUpdateDto, Appointment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                !IsDefaultOrEmptyValue(srcMember)));

        CreateMap<DoctorUpdateDto, Doctor>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                !IsDefaultOrEmptyValue(srcMember)));

        CreateMap<PatientUpdateDto, Patient>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                !IsDefaultOrEmptyValue(srcMember)));

        CreateMap<SpecializationUpdateDto, Specialization>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                !IsDefaultOrEmptyValue(srcMember)));
    }

    /// <summary>
    /// Determines whether a value is considered default or empty for mapping purposes.
    /// Used to prevent mapping of default values during partial updates.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is considered default or empty; otherwise, false.</returns>
    private static bool IsDefaultOrEmptyValue(object value)
    {
        if (value == null) return true;

        var type = value.GetType();

        if (type == typeof(string))
            return string.IsNullOrEmpty((string)value);

        if (type == typeof(uint) || type == typeof(int) ||
            type == typeof(uint?) || type == typeof(int?))
            return value.Equals(0) || value.Equals((uint)0);

        if (type == typeof(bool) || type == typeof(bool?))
            return value.Equals(false);

        if (type == typeof(DateTime) || type == typeof(DateTime?))
            return value.Equals(DateTime.MinValue);

        if (type == typeof(DateOnly) || type == typeof(DateOnly?))
            return value.Equals(DateOnly.MinValue);

        if (type.IsValueType)
            return value.Equals(Activator.CreateInstance(type));

        return false;
    }
}