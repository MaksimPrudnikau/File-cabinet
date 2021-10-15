using System;

namespace FileCabinetApp
{
    public class CustomDateOfBirthValidator : IRecordValidator
    {
        /// <summary>
        /// Validate date of birth in format "dd/MM/yyyy"
        /// </summary>
        /// <param name="dateOfBirth">entered date of birth</param>
        /// <exception cref="ArgumentNullException">Date of birth is null or whitespace</exception>
        /// <exception cref="ArgumentException">Date of birth is not in dd/mm/yyyy format.
        /// Date of birth is less than 01-Jan-1950 or greater than current date time</exception>
        public static ValidationResult Validate(DateTime dateOfBirth)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{dateOfBirth}"};

            if (dateOfBirth < FileCabinetConsts.MinimalDateTime)
            {
                result.Message = RecordValidatorConsts.DateOfBirthIsLessThanMinimal;
                return result;
            }

            if (dateOfBirth > FileCabinetConsts.MaximalDateTime)
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
            
            if (dateOfBirth < FileCabinetConsts.MinimalDateTime)
            {
                throw new ArgumentException(RecordValidatorConsts.DateOfBirthIsLessThanMinimal);
            }

            if (dateOfBirth > FileCabinetConsts.MaximalDateTime)
            {
                throw new ArgumentException(RecordValidatorConsts.DateOfBirthIsGreaterThanMaximal);
            }
        }
    }
}