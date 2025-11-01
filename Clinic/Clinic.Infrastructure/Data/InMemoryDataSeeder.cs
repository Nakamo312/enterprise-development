using Clinic.Domain.Models;
using Clinic.Domain.Data;
using Clinic.Infrastructure.Data.Interfaces;
using Clinic.Infrastructure.Repositories.Interfaces;

namespace Clinic.Infrastructure.Data;

/// <summary>
/// In-memory implementation of data seeder for testing and development purposes.
/// Seeds initial data into in-memory repositories.
/// </summary>
public class InMemoryDataSeeder(
        IRepository<Patient> patientRepository,
        IRepository<Doctor> doctorRepository,
        IRepository<Specialization> specializationRepository,
        IRepository<Appointment> appointmentRepository,
        DataSeed data
    ) : IDataSeeder
{
    /// <summary>
    /// Seeds the in-memory repositories with initial test data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SeedAsync()
    {
        var patientTasks = data.Patients.Select(p => patientRepository.CreateAsync(p)).ToList();
        await Task.WhenAll(patientTasks);

        var specializationTasks = data.Specializations.Select(s => specializationRepository.CreateAsync(s)).ToList();
        await Task.WhenAll(specializationTasks);

        var doctorTasks = data.Doctors.Select(d => doctorRepository.CreateAsync(d)).ToList();
        await Task.WhenAll(doctorTasks);

        var appointmentTasks = data.Appointments.Select(a => appointmentRepository.CreateAsync(a)).ToList();
        await Task.WhenAll(appointmentTasks);
    }

    /// <summary>
    /// Clears all data from the in-memory repositories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ClearAsync()
    {
        var patients = (await patientRepository.GetAsync()).ToList();
        var patientDeleteTasks = patients.Select(p => patientRepository.DeleteAsync(p.Id)).ToList();
        await Task.WhenAll(patientDeleteTasks);

        var appointments = (await appointmentRepository.GetAsync()).ToList();
        var appointmentDeleteTasks = appointments.Select(a => appointmentRepository.DeleteAsync(a.Id)).ToList();
        await Task.WhenAll(appointmentDeleteTasks);

        var doctors = (await doctorRepository.GetAsync()).ToList();
        var doctorDeleteTasks = doctors.Select(d => doctorRepository.DeleteAsync(d.Id)).ToList();
        await Task.WhenAll(doctorDeleteTasks);

        var specializations = (await specializationRepository.GetAsync()).ToList();
        var specializationDeleteTasks = specializations.Select(s => specializationRepository.DeleteAsync(s.Id)).ToList();
        await Task.WhenAll(specializationDeleteTasks);
    }
}