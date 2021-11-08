using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace UI.ValidationRules
{
    public class IsIpAddressValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((value ?? "").ToString()))
            {
                return new ValidationResult(false, "Field is required");
            }
            else
            {
                if (Regex.IsMatch(value.ToString(), @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]).){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"))
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, "Invalid format");
                }
            }
        }
    }
}