using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace практическая_2.Services.ValidationAttributes
{
    public class BirthDateRangeAttribute: ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public BirthDateRangeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            
            if (!(value is DateTime date))
                return new ValidationResult("Некорректный формат даты");

            var today = DateTime.Today;
            var minDate = today.AddYears(-_minAge);
            var maxdate = today.AddYears(-_maxAge);

            if(date > maxdate || date < minDate)
            {
                return new ValidationResult(
                    $"Возраст должен быть в диапазоне между {_minAge} и {_maxAge} годами (минимальная дата: {minDate:d}, максимальная дата: {maxdate:d}");
            }

            return ValidationResult.Success;
        }
    }
}
