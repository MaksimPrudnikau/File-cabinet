using System;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
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
        /// <exception cref="ArgumentException">Entered name is null or whitespace or it`s length is less than 2 or greater than 60</exception>
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
            return result;
        }

        /// <summary>
        /// Validate date of birth in format "dd/MM/yyyy"
        /// </summary>
        /// <param name="dateOfBirth">entered date of birth</param>
        /// <exception cref="ArgumentNullException">Date of birth is null or whitespace</exception>
        /// <exception cref="ArgumentException">Date of birth is not in dd/mm/yyyy format.
        /// Date of birth is less than 01-Jan-1950 or greater than current date time</exception>
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

        /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        public ValidationResult JobExperienceValidator(short jobExperience)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{jobExperience}"};

            if (jobExperience < FileCabinetConsts.MinimalJobExperience)
            {
                result.Message = RecordValidatorConsts.JobExperienceIsLessThanMinimal;
                return result;
            }

            if (jobExperience > FileCabinetConsts.MaximalJobExperience)
            {
                result.Message = RecordValidatorConsts.JobExperienceIsGreaterThanMaximal;
                return result;
            }

            result.Parsed = true;
            return result;
        }
        
        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        public ValidationResult WageValidator(decimal wage)
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
        
        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        public ValidationResult RankValidator(char rank)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{rank}"};

            if (Array.IndexOf(FileCabinetConsts.Grades, rank) == -1)
            {
                result.Message = RecordValidatorConsts.RankIsNotDefinedInGrades;
                return result;
            }

            result.Parsed = true;
            return result;
        }
    }
}