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
        var tasks = new List<Task>();

        data.Patients.ForEach(p => tasks.Add(patientRepository.CreateAsync(p)));
        data.Specializations.ForEach(s => tasks.Add(specializationRepository.CreateAsync(s)));
        data.Doctors.ForEach( d=> tasks.Add(doctorRepository.CreateAsync(d)));
        data.Appointments.ForEach(a => tasks.Add(appointmentRepository.CreateAsync(a)));

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Clears all data from the in-memory repositories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ClearAsync()
    {
        var tasks = new List<Task>();

        var patients = await patientRepository.GetAsync();
        var appointments = await appointmentRepository.GetAsync();
        var doctors = await doctorRepository.GetAsync();
        var specializations = await specializationRepository.GetAsync();

        appointments.ToList().ForEach(a => tasks.Add(appointmentRepository.DeleteAsync(a.Id)));
        doctors.ToList().ForEach(d => tasks.Add(doctorRepository.DeleteAsync(d.Id)));
        patients.ToList().ForEach(p => tasks.Add(patientRepository.DeleteAsync(p.Id)));
        specializations.ToList().ForEach(s => tasks.Add(specializationRepository.DeleteAsync(s.Id)));

        await Task.WhenAll(tasks);
    }
}