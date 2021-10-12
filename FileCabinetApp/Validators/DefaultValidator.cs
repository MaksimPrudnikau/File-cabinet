using System;

namespace FileCabinetApp
{
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validate source id
        /// </summary>
        /// <param name="id">Source id</param>
        public ValidationResult IdValidator(int id)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{id}"};

            if (id < 0)
            {
                result.Message = RecordValidatorConsts.IdIsLessThenZero;
                return result;
            }

            result.Parsed = true;
            return result;
        }
        
        /// <summary>
        /// Validate name either first or last
        /// </summary>
        /// <param name="name">first or last name</param>
        public ValidationResult NameValidator(string name)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = name};
            
            if (string.IsNullOrWhiteSpace(name))
            {
                result.Message = RecordValidatorConsts.NameIsNullOrWhiteSpace;
                return result;
            }

            if (name.Length is < 2 or > 60)
            {
                result.Message = RecordValidatorConsts.NameWrongLength;
                return result;
            }

            foreach (var item in name)
            {
                if (!char.IsLetter(item))
                {
                    result.Message = RecordValidatorConsts.TheNameIsNotLettersOnly;
                    return result;
                }
            }

            result.Parsed = true;
            return result;        }

        /// <summary>
        /// Validate date of birth in format "dd/MM/yyyy"
        /// </summary>
        /// <param name="dateOfBirth">entered date of birth</param>
        public ValidationResult DateOfBirthValidator(DateTime dateOfBirth)
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

        public ValidationResult JobExperienceValidator(short jobExperience)
        {
            return new ValidationResult();
        }

        public ValidationResult WageValidator(decimal wage)
        {
            return new ValidationResult();
        }

        public ValidationResult RankValidator(char rank)
        {
            return new ValidationResult();
        }
    }
}