using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace практическая_2.Services.ValidationAttributes
{
    public class RequiredIfFilledAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is string str && string.IsNullOrWhiteSpace(str))
                return ValidationResult.Success;

            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(ErrorMessage ?? "Поле обязательно для заполнения.");
            }

            return ValidationResult.Success;
        }
    }
}
