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
            if (id < 0)
            {
                return new ValidationResult
                {
                    Parsed = false, StringRepresentation = $"{id}",
                    Message = RecordValidatorConsts.IdIsLessThenZero

                };
            }

            return  new ValidationResult {Parsed = true, StringRepresentation = $"{id}"};
        }
        
        /// <summary>
        /// Validate name either first or last
        /// </summary>
        /// <param name="name">first or last name</param>
        public ValidationResult NameValidator(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length is < 2 or > 60)
            {
                return new ValidationResult
                {
                    Parsed = false, StringRepresentation = name, 
                    Message = RecordValidatorConsts.NameIsNullOrWhiteSpace
                };
            }

            foreach (var item in name)
            {
                if (!char.IsLetter(item))
                {
                    return new ValidationResult
                    {
                        Parsed = false, StringRepresentation = name,
                        Message = RecordValidatorConsts.TheNameIsNotLettersOnly
                    };
                }
            }

            return new ValidationResult {Parsed = true, StringRepresentation = name};
        }

        /// <summary>
        /// Validate date of birth in format "dd/MM/yyyy"
        /// </summary>
        /// <param name="dateOfBirth">entered date of birth</param>
        public ValidationResult DateOfBirthValidator(DateTime dateOfBirth)
        {
            if (dateOfBirth < FileCabinetConsts.MinimalDateTime)
            {
                return new ValidationResult
                {
                    Parsed = false, StringRepresentation = $"{dateOfBirth}",
                    Message = RecordValidatorConsts.DateOfBirthIsLessThanMinimal
                };
            }

            if (dateOfBirth > FileCabinetConsts.MaximalDateTime)
            {
                return new ValidationResult
                {
                    Parsed = false, StringRepresentation = $"{dateOfBirth}",
                    Message = RecordValidatorConsts.DateOfBirthIsGreaterThanMaximal
                };
            }
            
            return new ValidationResult {Parsed = true, StringRepresentation = $"{dateOfBirth}"};
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