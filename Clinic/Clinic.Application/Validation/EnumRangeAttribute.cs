using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.Validation;

/// <summary>
/// Validation attribute that ensures a value is a valid member of the specified enumeration type.
/// </summary>
public class EnumRangeAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    /// <summary>
    /// Initializes a new instance of the EnumRangeAttribute class.
    /// </summary>
    /// <param name="enumType">The type of enumeration to validate against.</param>
    public EnumRangeAttribute(Type enumType)
    {
        _enumType = enumType;
    }

    /// <summary>
    /// Validates that the specified value is a defined member of the enumeration.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="validationContext">The context information about the validation operation.</param>
    /// <returns>ValidationResult.Success if valid; otherwise, an error message.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        if (!Enum.IsDefined(_enumType, value))
        {
            var validValues = string.Join(", ", Enum.GetNames(_enumType));
            return new ValidationResult($"The field {validationContext.DisplayName} must be one of: {validValues}");
        }

        return ValidationResult.Success;
    }
}