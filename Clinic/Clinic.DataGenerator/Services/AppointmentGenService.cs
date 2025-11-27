using Bogus;
using Clinic.Application.Dtos.Appointments;
using Clinic.DataGenerator.Services.Interfaces;

namespace Clinic.DataGenerator.Services;

/// <summary>
/// Service for generating <see cref="AppointmentCreateDto"/> test data
/// </summary>
public class AppointmentGenService : IDataGenService<AppointmentCreateDto>
{
    private readonly Faker<AppointmentCreateDto> _faker;
    private readonly List<Guid> _generatedIds = new();
    private List<Guid> _currentDoctorIds = new();
    private List<Guid> _currentPatientIds = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentGenService"/> class
    /// </summary>
    public AppointmentGenService()
    {
        _faker = new Faker<AppointmentCreateDto>("ru")
            .RuleFor(a => a.DateTime, f => f.Date.Between(DateTime.Now.AddDays(1), DateTime.Now.AddDays(90)))
            .RuleFor(a => a.RoomNumber, f => f.Random.Number(100, 500).ToString())
            .RuleFor(a => a.IsRepeated, f => f.Random.Bool(0.3f))
            .RuleFor(a => a.PatientId, f =>
            {
                if (!_currentPatientIds.Any())
                    throw new InvalidOperationException("No patient IDs available");
                return f.PickRandom(_currentPatientIds);
            })
            .RuleFor(a => a.DoctorId, f =>
            {
                if (!_currentDoctorIds.Any())
                    throw new InvalidOperationException("No doctor IDs available");
                return f.PickRandom(_currentDoctorIds);
            });
    }

    /// <summary>
    /// Generates a single appointment
    /// </summary>
    /// <returns>Generated appointment data</returns>
    /// <exception cref="InvalidOperationException">Thrown when no doctor or patient IDs are available</exception>
    public AppointmentCreateDto Generate()
    {
        if (!_currentDoctorIds.Any() || !_currentPatientIds.Any())
            throw new InvalidOperationException("No doctor or patient IDs available. Call SetDependencies first.");

        var appointment = _faker.Generate();
        var id = Guid.NewGuid();
        _generatedIds.Add(id);
        return appointment;
    }

    /// <summary>
    /// Generates multiple appointments
    /// </summary>
    /// <param name="count">Number of appointments to generate</param>
    /// <returns>Collection of generated appointment data</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when count is less than 1</exception>
    /// <exception cref="InvalidOperationException">Thrown when no doctor or patient IDs are available</exception>
    public IEnumerable<AppointmentCreateDto> Generate(int count)
    {
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0");
        if (!_currentDoctorIds.Any() || !_currentPatientIds.Any())
            throw new InvalidOperationException("No doctor or patient IDs available. Call SetDependencies first.");

        var appointments = _faker.Generate(count).ToList();
        var ids = Enumerable.Range(0, count).Select(_ => Guid.NewGuid()).ToList();
        _generatedIds.AddRange(ids);
        return appointments;
    }

    /// <summary>
    /// Sets the current doctor and patient IDs to use for generation
    /// </summary>
    /// <param name="doctorIds">List of doctor IDs</param>
    /// <param name="patientIds">List of patient IDs</param>
    public void SetDependencies(List<Guid> doctorIds, List<Guid> patientIds)
    {
        _currentDoctorIds = doctorIds ?? new List<Guid>();
        _currentPatientIds = patientIds ?? new List<Guid>();
    }

    /// <summary>
    /// Gets the list of generated IDs for tracking purposes
    /// </summary>
    /// <returns>List of generated GUIDs</returns>
    public List<Guid> GetGeneratedIds() => _generatedIds.ToList();
}