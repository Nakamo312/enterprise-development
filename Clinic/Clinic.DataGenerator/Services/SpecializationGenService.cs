using Clinic.Application.Dtos.Specializations;
using Clinic.DataGenerator.Services.Interfaces;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Clinic.DataGenerator.Services;

/// <summary>
/// Service for generating <see cref="SpecializationCreateDto"/> test data
/// </summary>
public class SpecializationGenService : IDataGenService<SpecializationCreateDto>, ICreationConfirmable
{
    private readonly ConcurrentQueue<string> _availableSpecializations;
    private readonly ConcurrentDictionary<string, string> _pendingSpecializations;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public int RemainingCount => _availableSpecializations.Count + _pendingSpecializations.Count;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecializationGenService"/> class
    /// </summary>
    public SpecializationGenService()
    {
        var specializations = new[]
        {
            "Cardiology", "Dermatology", "Neurology", "Pediatrics", "Surgery",
            "Orthopedics", "Ophthalmology", "Psychiatry", "Endocrinology",
            "Gastroenterology", "Urology", "Oncology", "Radiology", "Pathology"
        };
        var random = new Random();
        _availableSpecializations = new ConcurrentQueue<string>(
            specializations.OrderBy(x => random.Next())
        );
        _pendingSpecializations = new ConcurrentDictionary<string, string>();
    }

    /// <inheritdoc/>
    public SpecializationCreateDto Generate()
    {
        if (RemainingCount == 0)
        {
            throw new InvalidOperationException("No more unique specializations available");
        }

        if (!_availableSpecializations.TryDequeue(out var name))
        {
            throw new InvalidOperationException("Failed to dequeue specialization");
        }

        var dto = new SpecializationCreateDto { Name = name };
        var hash = ComputeDataHash(dto);
        _pendingSpecializations[hash] = name;

        return dto;
    }

    /// <inheritdoc/>
    public IEnumerable<SpecializationCreateDto> Generate(int count)
    {
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0");

        if (count > RemainingCount)
        {
            throw new ArgumentOutOfRangeException(nameof(count),
                $"Cannot generate more than {RemainingCount} unique specializations");
        }

        var result = new List<SpecializationCreateDto>();
        for (var i = 0; i < count; i++)
        {
            result.Add(Generate());
        }
        return result;
    }

    /// <inheritdoc/>
    public void ConfirmCreation(string dataHash)
    {
        _pendingSpecializations.TryRemove(dataHash, out _);
    }

    /// <inheritdoc/>
    public void ReturnToAvailable(string dataHash)
    {
        if (_pendingSpecializations.TryRemove(dataHash, out var name))
        {
            _availableSpecializations.Enqueue(name);
        }
    }

    /// <summary>
    /// Computes SHA256 hash from specialization DTO
    /// </summary>
    private static string ComputeDataHash(SpecializationCreateDto dto)
    {
        var json = JsonSerializer.Serialize(dto, _jsonSerializerOptions);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Gets the list of generated IDs for tracking purposes
    /// </summary>
    /// <returns>List of generated GUIDs</returns>
    public List<Guid> GetGeneratedIds() => [];
}