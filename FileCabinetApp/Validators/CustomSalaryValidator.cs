using System;

namespace FileCabinetApp
{
    public class CustomSalaryValidator : IRecordValidator
    {
        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        public static ValidationResult Validate(decimal wage)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{wage}"};

            if (wage < FileCabinetConsts.MinimalWage)
            {
                result.Message = RecordValidatorConsts.WageIsLessThanMinimal;
                return result;
            }

            result.Parsed = true;
            return result;
        }

        public void Validate(FileCabinetRecord record)
        {
            if (record.Salary < FileCabinetConsts.MinimalWage)
            {
                throw new ArgumentException(RecordValidatorConsts.WageIsLessThanMinimal);
            }
        }
    }
}