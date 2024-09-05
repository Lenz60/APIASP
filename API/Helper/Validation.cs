namespace API.Helper;
using System.ComponentModel.DataAnnotations;

public class NotNullOrEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult($"Kolom {validationContext.DisplayName} dibutuhkan.");
        }

        return ValidationResult.Success;
    }
}
