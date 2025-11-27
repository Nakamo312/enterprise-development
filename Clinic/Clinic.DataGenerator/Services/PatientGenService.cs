using Bogus;
using Clinic.Application.Dtos.Patients;
using Clinic.Domain.Enums;
using Clinic.DataGenerator.Services.Interfaces;

namespace Clinic.DataGenerator.Services;

/// <summary>
/// Service for generating <see cref="PatientCreateDto"/> test data
/// </summary>
public class PatientGenService : IDataGenService<PatientCreateDto>
{
    private readonly Faker<PatientCreateDto> _faker;
    private readonly List<Guid> _generatedIds = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientGenService"/> class
    /// </summary>
    public PatientGenService()
    {
        _faker = new Faker<PatientCreateDto>("ru")
            .RuleFor(p => p.PassportNumber, f => f.Random.Replace("####-######"))
            .RuleFor(p => p.FullName, f => f.Name.FullName())
            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
            .RuleFor(p => p.DateOfBirth, f => DateOnly.FromDateTime(f.Date.Between(DateTime.Now.AddYears(-90), DateTime.Now.AddYears(-18))))
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .RuleFor(p => p.BloodGroup, f => f.PickRandom<BloodGroup>())
            .RuleFor(p => p.RhFactor, f => f.PickRandom<RhFactor>())
            .RuleFor(p => p.ContactPhone, f => f.Phone.PhoneNumberFormat());
    }

    /// <inheritdoc/>
    public PatientCreateDto Generate()
    {
        var patient = _faker.Generate();
        var id = Guid.NewGuid();
        _generatedIds.Add(id);
        return patient;
    }

    /// <inheritdoc/>
    public IEnumerable<PatientCreateDto> Generate(int count)
    {
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0");

        var patients = _faker.Generate(count).ToList();
        var ids = Enumerable.Range(0, count).Select(_ => Guid.NewGuid()).ToList();
        _generatedIds.AddRange(ids);
        return patients;
    }

    /// <summary>
    /// Gets the list of generated IDs for tracking purposes
    /// </summary>
    /// <returns>List of generated GUIDs</returns>
    public List<Guid> GetGeneratedIds() => _generatedIds.ToList();
}