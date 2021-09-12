using System;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validate name either first or last
        /// </summary>
        /// <param name="name">first or last name</param>
        /// <exception cref="ArgumentException">Entered name is null or whitespace or it`s length is less than 2 or greater than 60</exception>
        public ValidationResult NameValidator(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length is >= 2 and <= 60
                ? new ValidationResult {Parsed = true, StringRepresentation = name}
                : new ValidationResult {Parsed = false, StringRepresentation = name};
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
            var minimalDateTime = new DateTime(1950, 1, 1);
            var maximumDateTime = DateTime.Now;

            return dateOfBirth >= minimalDateTime && dateOfBirth <= maximumDateTime
                ? new ValidationResult {Parsed = true, StringRepresentation = $"{dateOfBirth}"}
                : new ValidationResult {Parsed = false, StringRepresentation = $"{dateOfBirth}"};
        }

        /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        public ValidationResult JobExperienceValidator(short jobExperience)
        {
            return jobExperience is >= 0 and < 100
                ? new ValidationResult {Parsed = true, StringRepresentation = $"{jobExperience}"}
                : new ValidationResult {Parsed = false, StringRepresentation = $"{jobExperience}"};
        }
        
        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        public ValidationResult WageValidator(decimal wage)
        {
            const int minimalWage = 250;
            return wage >= minimalWage
                ? new ValidationResult {Parsed = true, StringRepresentation = $"{wage}"}
                : new ValidationResult {Parsed = false, StringRepresentation = $"{wage}"};
        }
        
        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        public ValidationResult RankValidator(char rank)
        {
            var grades = new []{'F', 'D', 'C', 'B', 'A'};

            if (Array.IndexOf(grades, rank) == -1)
            {
                throw new ArgumentException("Rank is not defined in current rank system [F..A]");
            }

            return Array.IndexOf(grades, rank) > -1
                ? new ValidationResult {Parsed = true, StringRepresentation = $"{rank}"}
                : new ValidationResult {Parsed = false, StringRepresentation = $"{rank}"};
        }
    }
}