using System;

namespace FileCabinetApp
{
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validate name either first or last
        /// </summary>
        /// <param name="name">first or last name</param>
        /// <exception cref="ArgumentException">Entered name is null or whitespace or it`s length is less than 2 or greater than 60</exception>
        public ValidationResult NameValidator(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length is < 2 or > 60)
            {
                return new ValidationResult {Parsed = false, StringRepresentation = name};
            }

            foreach (var item in name)
            {
                if (!char.IsLetter(item))
                {
                    return new ValidationResult {Parsed = false, StringRepresentation = name};
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
            var minimalDateTime = new DateTime(1950, 1, 1);
            var maximumDateTime = DateTime.Now;

            return dateOfBirth >= minimalDateTime && dateOfBirth <= maximumDateTime
                ? new ValidationResult {Parsed = true, StringRepresentation = $"{dateOfBirth}"}
                : new ValidationResult {Parsed = false, StringRepresentation = $"{dateOfBirth}"};
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