using Clinic.Domain.Enums;
using Clinic.Application.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Clinic.Application.Dtos.Patients;

/// <summary>
/// Data transfer object for updating an existing patient.
/// </summary>
public class PatientUpdateDto : PatientCreateDto;
