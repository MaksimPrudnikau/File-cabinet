using System;
using System.Globalization;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
    {
        public void ValidateParameters(Parameter parameters)
        {
            NameValidator(parameters.FirstName);
            NameValidator(parameters.LastName);
            DateOfBirthValidator(parameters.DateOfBirth);
            JobExperienceValidator(parameters.JobExperience);
            WageValidator(parameters.Wage);
            RankValidator(parameters.Rank);
        }
        
        /// <summary>
        /// Validate name either first or last
        /// </summary>
        /// <param name="name">first or last name</param>
        /// <exception cref="ArgumentException">Entered name is null or whitespace or it`s length is less than 2 or greater than 60</exception>
        private static void NameValidator(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length is < 2 or > 60 )
            {
                throw new ArgumentException(
                    "Entered name is null or whitespace or it`s length is less than 2 or greater than 60");
            }
        }

        /// <summary>
        /// Validate date of birth in format "dd/MM/yyyy"
        /// </summary>
        /// <param name="source">entered date of birth</param>
        /// <exception cref="ArgumentNullException">Date of birth is null or whitespace</exception>
        /// <exception cref="ArgumentException">Date of birth is not in dd/mm/yyyy format.
        /// Date of birth is less than 01-Jan-1950 or greater than current date time</exception>
        private static void DateOfBirthValidator(string source)
        {
            const string inputDateFormat = "dd/MM/yyyy";
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source), EnglishSource.Date_birth_is_null_or_whitespace);
            }

            if (!DateTime.TryParseExact(source, inputDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var dateOfBirth))
            {
                throw new ArgumentException("Date of birth is not in dd/mm/yyyy format");
            }

            var minimalDateTime = new DateTime(1950, 1, 1);
            var maximumDateTime = DateTime.Now;
            if (dateOfBirth < minimalDateTime || dateOfBirth > maximumDateTime)
            {
                throw new ArgumentException("Date of birth is less than 01-Jan-1950 or greater than current date time");
            }
        }
        
        /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        private static void JobExperienceValidator(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!short.TryParse(source, out var jobExperience))
            {
                throw new ArgumentException(
                    "Job experience is not an integer or less than zero or greater than short.MaxValue");
            }

            if (jobExperience is < 0 or > 100)
            {
                throw new ArgumentException("Job experience is less than zero or greater than 100");
            }
        }
        
        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        private static void WageValidator(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            

            if (!decimal.TryParse(source, out var wage))
            {
                throw new ArgumentException(
                    "Wage is not an integer or greater than decimal.MaxValue");
            }

            if (wage < 0)
            {
                throw new ArgumentException("Wage is less than zero");
            }
        }
        
        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        private static void RankValidator(string source)
        {
            var grades = new []{'F', 'D', 'C', 'B', 'A'};
            
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            if (source.Length != 1 || Array.IndexOf(grades, source[0]) == -1)
            {
                throw new ArgumentException("Rank is not defined in current rank system [F..A]");
            }
        }
    }
}