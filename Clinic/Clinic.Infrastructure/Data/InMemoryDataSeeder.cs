using Clinic.Infrastructure.Data.Interfaces;
using Clinic.Domain.Models;
using Clinic.Infrastructure.Repositories;

namespace Clinic.Infrastructure.Data;

/// <summary>
/// In-memory implementation of data seeder for testing and development purposes.
/// Seeds initial data into in-memory repositories.
/// </summary>
public class InMemoryDataSeeder(
        IRepository<Patient> patientRepository,
        IRepository<Doctor> doctorRepository,
        IRepository<Specialization> specializationRepository,
        IRepository<Appointment> appointmentRepository
    ) : IDataSeeder
{
    private readonly IRepository<Patient> _patientRepository = patientRepository;
    private readonly IRepository<Doctor> _doctorRepository = doctorRepository;
    private readonly IRepository<Specialization> _specializationRepository = specializationRepository;
    private readonly IRepository<Appointment> _appointmentRepository = appointmentRepository;

    /// <summary>
    /// Seeds the in-memory repositories with initial test data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SeedAsync()
    {

        foreach (var patient in DataSeed.Patients)
        {
            await _patientRepository.CreateAsync(patient);
        }

        foreach (var specialization in DataSeed.Specializations)
        {
            await _specializationRepository.CreateAsync(specialization);
        }

        foreach (var doctor in DataSeed.Doctors)
        {
            await _doctorRepository.CreateAsync(doctor);
        }

        foreach (var appointment in DataSeed.Appointments)
        {
            await _appointmentRepository.CreateAsync(appointment);
        }
    }

    /// <summary>
    /// Clears all data from the in-memory repositories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ClearAsync()
    {
        var patients = await _patientRepository.GetAsync();
        foreach (var patient in patients)
        {
            await _patientRepository.DeleteAsync(patient.Id);
        }

        var appointments = await _appointmentRepository.GetAsync();
        foreach (var appointment in appointments)
        {
            await _appointmentRepository.DeleteAsync(appointment.Id);
        }

        var doctors = await _doctorRepository.GetAsync();
        foreach (var doctor in doctors)
        {
            await _doctorRepository.DeleteAsync(doctor.Id);
        }

        var specializations = await _specializationRepository.GetAsync();
        foreach (var specialization in specializations)
        {
            await _specializationRepository.DeleteAsync(specialization.Id);
        }
    }
}