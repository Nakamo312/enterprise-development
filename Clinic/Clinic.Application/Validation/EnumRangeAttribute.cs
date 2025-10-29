using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.Validation;

/// <summary>
/// Validation attribute that ensures a value is a valid member of the specified enumeration type.
/// Works with both string and integer enum values, compatible with JsonStringEnumConverter.
/// </summary>
/// <param name="enumType">The type of the enumeration to validate against</param>
public class EnumRangeAttribute(Type enumType) : ValidationAttribute
{
    /// <summary>
    /// Validates that the specified value is a defined member of the enumeration.
    /// Supports both string names and integer values of the enum.
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The context information about the validation operation</param>
    /// <returns>ValidationResult.Success if valid; otherwise, an error message</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        if (value is string stringValue)
        {
            if (Enum.TryParse(enumType, stringValue, true, out var parsedEnum) &&
                Enum.IsDefined(enumType, parsedEnum))
            {
                return ValidationResult.Success;
            }

            var validValues = string.Join(", ", Enum.GetNames(enumType));
            return new ValidationResult($"The field {validationContext.DisplayName} must be one of: {validValues}. Received: '{stringValue}'");
        }

        if (!Enum.IsDefined(enumType, value))
        {
            var validValues = string.Join(", ", Enum.GetNames(enumType));
            return new ValidationResult($"The field {validationContext.DisplayName} must be one of: {validValues}. Received: {value}");
        }

        return ValidationResult.Success;
    }
}