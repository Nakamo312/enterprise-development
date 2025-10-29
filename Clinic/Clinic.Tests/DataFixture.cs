using Clinic.Domain.Models;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.Data.Interfaces;
using Clinic.Infrastructure.Repositories;

namespace Clinic.Tests;

/// <summary>
/// Provides a comprehensive set of test data fixtures seeded from in-memory repositories.
/// This class ensures that test data is consistent with the actual repository state.
/// </summary>
public class DataFixture : IAsyncLifetime
{
    private readonly IRepository<Patient> _patientRepository;
    private readonly IRepository<Doctor> _doctorRepository;
    private readonly IRepository<Specialization> _specializationRepository;
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IDataSeeder _dataSeeder;

    /// <summary>
    /// Gets the list of patients from the in-memory repository.
    /// </summary>
    public List<Patient> Patients { get; private set; } = new();

    /// <summary>
    /// Gets the list of doctors from the in-memory repository.
    /// </summary>
    public List<Doctor> Doctors { get; private set; } = new();

    /// <summary>
    /// Gets the list of specializations from the in-memory repository.
    /// </summary>
    public List<Specialization> Specializations { get; private set; } = new();

    /// <summary>
    /// Gets the list of appointments from the in-memory repository.
    /// </summary>
    public List<Appointment> Appointments { get; private set; } = new();

    public DataFixture()
    {
        _patientRepository = new InMemoryRepository<Patient>();
        _doctorRepository = new InMemoryRepository<Doctor>();
        _specializationRepository = new InMemoryRepository<Specialization>();
        _appointmentRepository = new InMemoryRepository<Appointment>();

        _dataSeeder = new InMemoryDataSeeder(
            _patientRepository,
            _doctorRepository,
            _specializationRepository,
            _appointmentRepository);
    }

    /// <summary>
    /// Initializes the test data by seeding repositories and loading data from them.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _dataSeeder.SeedAsync();
        Patients = (await _patientRepository.GetAsync()).ToList();
        Doctors = (await _doctorRepository.GetAsync()).ToList();
        Specializations = (await _specializationRepository.GetAsync()).ToList();
        Appointments = (await _appointmentRepository.GetAsync()).ToList();
    }

    /// <summary>
    /// Cleans up test data.
    /// </summary>
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}