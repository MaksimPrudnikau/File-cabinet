using System;

namespace FileCabinetApp
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime _from;
        private readonly DateTime _to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            _from = from;
            _to = to;
        }
        
        /// <summary>
        /// Validate date of birth in format "dd/MM/yyyy"
        /// </summary>
        /// <param name="dateOfBirth">entered date of birth</param>
        /// <exception cref="ArgumentNullException">Date of birth is null or whitespace</exception>
        /// <exception cref="ArgumentException">Date of birth is not in dd/mm/yyyy format.
        /// Date of birth is less than 01-Jan-1950 or greater than current date time</exception>
        public ValidationResult Validate(DateTime dateOfBirth)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{dateOfBirth}"};

            if (dateOfBirth < _from)
            {
                result.Message = RecordValidatorConsts.DateOfBirthIsLessThanMinimal;
                return result;
            }

            if (dateOfBirth > _to)
            {
                result.Message = RecordValidatorConsts.DateOfBirthIsGreaterThanMaximal;
                return result;
            }

            result.Parsed = true;
            return result;
        }

        public void Validate(FileCabinetRecord record)
        {
            var dateOfBirth = record.DateOfBirth;
            
            if (dateOfBirth < _from)
            {
                throw new ArgumentException(RecordValidatorConsts.DateOfBirthIsLessThanMinimal);
            }

            if (dateOfBirth > _to)
            {
                throw new ArgumentException(RecordValidatorConsts.DateOfBirthIsGreaterThanMaximal);
            }
        }
    }
}