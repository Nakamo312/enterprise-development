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
    /// <summary>
    /// Seeds the in-memory repositories with initial test data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SeedAsync()
    {

        foreach (var patient in DataSeed.Patients)
        {
            await patientRepository.CreateAsync(patient);
        }

        foreach (var specialization in DataSeed.Specializations)
        {
            await specializationRepository.CreateAsync(specialization);
        }

        foreach (var doctor in DataSeed.Doctors)
        {
            await doctorRepository.CreateAsync(doctor);
        }

        foreach (var appointment in DataSeed.Appointments)
        {
            await appointmentRepository.CreateAsync(appointment);
        }
    }

    /// <summary>
    /// Clears all data from the in-memory repositories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ClearAsync()
    {
        var patients = await patientRepository.GetAsync();
        foreach (var patient in patients)
        {
            await patientRepository.DeleteAsync(patient.Id);
        }

        var appointments = await appointmentRepository.GetAsync();
        foreach (var appointment in appointments)
        {
            await appointmentRepository.DeleteAsync(appointment.Id);
        }

        var doctors = await doctorRepository.GetAsync();
        foreach (var doctor in doctors)
        {
            await doctorRepository.DeleteAsync(doctor.Id);
        }

        var specializations = await specializationRepository.GetAsync();
        foreach (var specialization in specializations)
        {
            await specializationRepository.DeleteAsync(specialization.Id);
        }
    }
}