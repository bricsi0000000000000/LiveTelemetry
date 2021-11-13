using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((value ?? "").ToString()))
            {
                return new ValidationResult(false, "Field is required");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
