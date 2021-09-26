using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules
{
    public class IsNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((value ?? "").ToString()))
            {
                return new ValidationResult(false, "Field is required.");
            }
            else
            {
                if (int.TryParse(value.ToString(), out int result))
                {
                    if (result > 0)
                    {
                        return ValidationResult.ValidResult;
                    }
                    else
                    {
                        return new ValidationResult(false, "Must be bigger than 0.");
                    }
                }
                else
                {
                    return new ValidationResult(false, "Number is required.");
                }
            }
        }
    }
}