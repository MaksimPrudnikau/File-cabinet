using System;

namespace FileCabinetApp
{
    public class DefaultDateOfBirthValidator : IRecordValidator
    {
        /// <summary>
        /// Validate date of birth in format "dd/MM/yyyy"
        /// </summary>
        /// <param name="dateOfBirth">entered date of birth</param>
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