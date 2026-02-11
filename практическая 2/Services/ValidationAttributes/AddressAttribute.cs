using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace практическая_2.Services.ValidationAttributes
{
    internal class AddressAttribute : ValidationAttribute
    {
        private readonly int _minLen;
        private readonly int _maxLen;

        public AddressAttribute(int minLen, int maxLen)
        {
            _minLen = minLen;
            _maxLen = maxLen;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is string str && string.IsNullOrWhiteSpace(str))
                return ValidationResult.Success;

            if (value is string address && !string.IsNullOrWhiteSpace(address))
            {
                if(address.Length > _maxLen)
                    return new ValidationResult(
                        $"Слишком длинный адресс: максимальная длина = {_maxLen} символов");
                else if(address.Length < _minLen)
                    return new ValidationResult(
                        $"Слишком короткий адресс: минимальная длина = {_minLen} символов");
                else return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}
