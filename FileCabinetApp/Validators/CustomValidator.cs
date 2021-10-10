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
        /// <exception cref="ArgumentException">Entered name is null or whitespace or it`s length is less than 2 or greater than 60</exception>
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
        /// <exception cref="ArgumentNullException">Date of birth is null or whitespace</exception>
        /// <exception cref="ArgumentException">Date of birth is not in dd/mm/yyyy format.
        /// Date of birth is less than 01-Jan-1950 or greater than current date time</exception>
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
            
            return new ValidationResult {Parsed = true, StringRepresentation = $"{dateOfBirth}"};        }

        /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        public ValidationResult JobExperienceValidator(short jobExperience)
        {
            if (jobExperience < FileCabinetConsts.MinimalJobExperience)
            {
                return new ValidationResult
                {
                    Parsed = false, StringRepresentation = $"{jobExperience}",
                    Message = RecordValidatorConsts.JobExperienceIsLessThanMinimal
                };
            }

            if (jobExperience > FileCabinetConsts.MaximalJobExperience)
            {
                return new ValidationResult
                {
                    Parsed = false, StringRepresentation = $"{jobExperience}",
                    Message = RecordValidatorConsts.JobExperienceIsGreaterThanMaximal
                };
            }

            return new ValidationResult {Parsed = true, StringRepresentation = $"{jobExperience}"};
        }
        
        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        public ValidationResult WageValidator(decimal wage)
        {
            if (wage < FileCabinetConsts.MinimalWage)
            {
                return new ValidationResult
                {
                    Parsed = false, StringRepresentation = $"{wage}",
                    Message = RecordValidatorConsts.WageIsLessThanMinimal
                };
            }
            
            return new ValidationResult {Parsed = true, StringRepresentation = $"{wage}"};
        }
        
        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        public ValidationResult RankValidator(char rank)
        {

            if (Array.IndexOf(FileCabinetConsts.Grades, rank) == -1)
            {
                return new ValidationResult
                {
                    Parsed = false, StringRepresentation = $"{rank}",
                    Message = RecordValidatorConsts.RankIsNotDefinedInGrades
                };
            }

            return new ValidationResult {Parsed = true, StringRepresentation = $"{rank}"};
        }
    }
}