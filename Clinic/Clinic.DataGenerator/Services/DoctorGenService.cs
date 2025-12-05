using Bogus;
using Clinic.Application.Dtos.Doctors;
using Clinic.DataGenerator.Services.Interfaces;

namespace Clinic.DataGenerator.Services;

/// <summary>
/// Service for generating <see cref="DoctorCreateDto"/> test data
/// </summary>
public class DoctorGenService : IDataGenService<DoctorCreateDto>
{
    private readonly Faker<DoctorCreateDto> _faker;
    private readonly List<Guid> _generatedIds = [];
    private List<Guid> _currentSpecializationIds = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorGenService"/> class
    /// </summary>
    public DoctorGenService()
    {
        _faker = new Faker<DoctorCreateDto>("ru")
            .RuleFor(d => d.PassportNumber, f => f.Random.Replace("####-######"))
            .RuleFor(d => d.FullName, f => f.Name.FullName())
            .RuleFor(d => d.YearOfBirth, f => (uint)f.Date.Between(DateTime.Now.AddYears(-65), DateTime.Now.AddYears(-25)).Year)
            .RuleFor(d => d.SpecializationId, f =>
            {
                if (!_currentSpecializationIds.Any())
                    throw new InvalidOperationException("No specialization IDs available");
                return f.PickRandom(_currentSpecializationIds);
            })
            .RuleFor(d => d.Experience, f => f.Random.Number(1, 40));
    }

    /// <inheritdoc/>
    public DoctorCreateDto Generate()
    {
        if (!_currentSpecializationIds.Any())
            throw new InvalidOperationException("No specialization IDs available. Call SetSpecializationIds first.");

        var doctor = _faker.Generate();
        var id = Guid.NewGuid();
        _generatedIds.Add(id);
        return doctor;
    }

    /// <inheritdoc/>
    public IEnumerable<DoctorCreateDto> Generate(int count)
    {
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0");
        if (!_currentSpecializationIds.Any())
            throw new InvalidOperationException("No specialization IDs available. Call SetSpecializationIds first.");

        var doctors = _faker.Generate(count).ToList();
        var ids = Enumerable.Range(0, count).Select(_ => Guid.NewGuid()).ToList();
        _generatedIds.AddRange(ids);
        return doctors;
    }

    /// <summary>
    /// Sets the current specialization IDs to use for generation
    /// </summary>
    public void SetSpecializationIds(List<Guid> specializationIds)
    {
        _currentSpecializationIds = specializationIds ?? [];
    }

    /// <summary>
    /// Gets the list of generated IDs for tracking purposes
    /// </summary>
    /// <returns>List of generated GUIDs</returns>
    public List<Guid> GetGeneratedIds() => _generatedIds.ToList();
}