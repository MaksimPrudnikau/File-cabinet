using System;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetDefaultService : FileCabinetService
    {
        protected override void ValidateParameters(Parameter parameters)
        {
            NameValidator(parameters.FirstName);
            NameValidator(parameters.LastName);
            DateOfBirthValidator(parameters.DateOfBirth);
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
    }
}