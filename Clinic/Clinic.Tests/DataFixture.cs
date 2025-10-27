using Clinic.Domain.Models;
using Clinic.Domain.Data;

namespace Clinic.Tests;

/// <summary>
/// Provides a comprehensive set of test data fixtures for the Clinic domain entities.
/// This class is intended for use in unit tests, integration tests, and development scenarios 
/// where consistent and realistic sample data is required.
/// </summary>
public class DataFixture
{
    /// <summary>
    /// Gets the list of patients.
    /// </summary>
    public List<Patient> Patients { get; } = DataSeed.Patients;

    /// <summary>
    /// Gets the list of doctors.
    /// </summary>
    public List<Doctor> Doctors { get; } = DataSeed.Doctors;

    /// <summary>
    /// Gets the list of specializations.
    /// </summary>
    public List<Specialization> Specializations { get; } = DataSeed.Specializations;

    /// <summary>
    /// Gets the list of appointments.
    /// </summary>
    public List<Appointment> Appointments { get; } = DataSeed.Appointments;
}