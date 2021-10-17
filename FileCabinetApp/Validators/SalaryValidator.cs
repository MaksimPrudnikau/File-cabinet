using System;

namespace FileCabinetApp
{
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal _minimal;

        public SalaryValidator(decimal minimalSalary)
        {
            _minimal = minimalSalary;
        }


        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        public ValidationResult Validate(decimal wage)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{wage}"};

            if (wage < _minimal)
            {
                result.Message = RecordValidatorConsts.WageIsLessThanMinimal;
                return result;
            }

            result.Parsed = true;
            return result;
        }

        public void Validate(FileCabinetRecord record)
        {
            if (record.Salary < _minimal)
            {
                throw new ArgumentException(RecordValidatorConsts.WageIsLessThanMinimal);
            }
        }
    }
}