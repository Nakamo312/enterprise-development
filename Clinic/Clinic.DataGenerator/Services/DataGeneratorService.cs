namespace Clinic.DataGenerator.Services;

/// <summary>
/// Main service for generating test data for all entity types
/// </summary>
public class DataGeneratorService
{
    private readonly EntityIdTracker _idTracker;
    private SpecializationGenService _specializationGenService;
    private PatientGenService _patientGenService;
    private DoctorGenService _doctorGenService;
    private AppointmentGenService _appointmentGenService;

    /// <summary>
    /// Gets the number of remaining specializations that can be generated
    /// </summary>
    public int SpecializationsRemainingCount => _specializationGenService.RemainingCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataGeneratorService"/> class
    /// </summary>
    /// <param name="idTracker">Entity ID tracker for dependency management</param>
    public DataGeneratorService(EntityIdTracker idTracker)
    {
        _idTracker = idTracker ?? throw new ArgumentNullException(nameof(idTracker));
        _specializationGenService = new SpecializationGenService();
        _patientGenService = new PatientGenService();
        _doctorGenService = new DoctorGenService();
        _appointmentGenService = new AppointmentGenService();

        _idTracker.RegisterConfirmableService("specialization", _specializationGenService);
    }

    /// <summary>
    /// Generates specialization entities
    /// </summary>
    /// <param name="count">Number of specializations to generate</param>
    /// <returns>List of generated specialization objects</returns>
    public List<object> GenerateSpecializations(int count)
    {
        var availableCount = _specializationGenService.RemainingCount;
        if (availableCount == 0) return [];

        var toGenerate = Math.Min(count, availableCount);
        var specializations = _specializationGenService.Generate(toGenerate).Cast<object>().ToList();
        return specializations;
    }

    /// <summary>
    /// Generates patient entities
    /// </summary>
    /// <param name="count">Number of patients to generate</param>
    /// <returns>List of generated patient objects</returns>
    public List<object> GeneratePatients(int count)
    {
        var patients = _patientGenService.Generate(count).Cast<object>().ToList();
        return patients;
    }

    /// <summary>
    /// Generates doctor entities with specialization dependencies
    /// </summary>
    /// <param name="count">Number of doctors to generate</param>
    /// <returns>List of generated doctor objects</returns>
    /// <exception cref="InvalidOperationException">Thrown when no specialization IDs are available</exception>
    public List<object> GenerateDoctors(int count)
    {
        var specIds = _idTracker.GetIds("specialization");

        if (!specIds.Any())
        {
            throw new InvalidOperationException("No specialization IDs available for generating doctors");
        }

        _doctorGenService.SetSpecializationIds(specIds);

        var doctors = _doctorGenService.Generate(count).Cast<object>().ToList();
        return doctors;
    }

    /// <summary>
    /// Generates appointment entities with doctor and patient dependencies
    /// </summary>
    /// <param name="count">Number of appointments to generate</param>
    /// <returns>List of generated appointment objects</returns>
    /// <exception cref="InvalidOperationException">Thrown when doctor or patient dependencies are missing</exception>
    public List<object> GenerateAppointments(int count)
    {
        var doctorIds = _idTracker.GetIds("doctor");
        var patientIds = _idTracker.GetIds("patient");

        if (!doctorIds.Any() || !patientIds.Any())
        {
            throw new InvalidOperationException($"Cannot create appointments - missing dependencies. Doctors: {doctorIds.Count}, Patients: {patientIds.Count}");
        }

        _appointmentGenService.SetDependencies(doctorIds, patientIds);

        var appointments = _appointmentGenService.Generate(count).Cast<object>().ToList();
        return appointments;
    }
}