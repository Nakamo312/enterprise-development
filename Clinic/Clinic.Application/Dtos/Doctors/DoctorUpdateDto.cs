using Clinic.Application.Dtos.Patients;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.Dtos.Doctors;

/// <summary>
/// Data transfer object for updating an existing doctor.
/// </summary>
public class DoctorUpdateDto : DoctorCreateDto;