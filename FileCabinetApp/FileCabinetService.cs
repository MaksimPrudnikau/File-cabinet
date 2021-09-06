using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public abstract class FileCabinetService
    {
        private const string inputFormat = "dd/MM/yyyy";
        private static readonly List<FileCabinetRecord> list = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new();
        private static readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new();

        /// <summary>
        /// The method create new record from input data and return its id
        /// </summary>
        /// <exception cref="ArgumentException"> either firstName or lastName are incorrect <see cref="InputFirstName"/>.
        ///  Date of birth is incorrect <see cref="InputDateOfBirth"/>.
        ///  Job experience is incorrect <see cref="InputJobExperience"/>.
        ///  Wage is incorrect <see cref="InputWage"/>.
        ///  Rank is incorrect <see cref="InputRank"/></exception>
        /// <returns>An id of current record</returns>
        public static int CreateRecord(Parameter parameters)
        {
            ValidateParameters(parameters);
            
            list.Add(new FileCabinetRecord
            {
                Id = parameters.Id,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = DateTime.ParseExact(parameters.DateOfBirth, inputFormat, CultureInfo.InvariantCulture),
                JobExperience = short.Parse(parameters.JobExperience, CultureInfo.InvariantCulture),
                Wage = decimal.Parse(parameters.Wage, CultureInfo.InvariantCulture),
                Rank = char.Parse(parameters.Rank)
            });
            
            AppendToAllDictionaries(list[^1]);
            return list[^1].Id;
        }

        /// <summary>
        /// Validate all parameters
        /// </summary>
        /// <param name="parameters">Entered parameters</param>
        protected static void ValidateParameters(Parameter parameters)
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

        /// <summary>
        /// Return a copy of internal service`s list 
        /// </summary>
        /// <returns><see cref="list"/> converted to char array</returns>
        public static IEnumerable<FileCabinetRecord> GetRecords()
        {
            return list.ToArray();
        }

        /// <summary>
        /// Returns number of records that the service stores
        /// </summary>
        /// <value>An ordinal number of the last record</value>
        public static int Stat => list.Count;
        
        public static void EditRecord(Parameter parameters)
        {
            ValidateParameters(parameters);
            
            if (parameters.Id >= list.Count)
            {
                Console.WriteLine(EnglishSource.record_not_found, parameters.Id);
                return;
            }

            list[parameters.Id].FirstName = parameters.FirstName;
            list[parameters.Id].LastName = parameters.LastName;
            list[parameters.Id].DateOfBirth =
                DateTime.ParseExact(parameters.DateOfBirth, inputFormat, CultureInfo.InvariantCulture);
            list[parameters.Id].JobExperience = short.Parse(parameters.JobExperience, CultureInfo.InvariantCulture);
            list[parameters.Id].Wage = decimal.Parse(parameters.Wage, CultureInfo.InvariantCulture);
            list[parameters.Id].Wage = char.Parse(parameters.Rank);

            RemoveFromAllDictionaries(list[parameters.Id]);

            AppendToAllDictionaries(list[parameters.Id]);
        }

        private static void AppendToAllDictionaries(FileCabinetRecord record)
        {
            AppendToFirstNameDictionary(record);
            AppendToLastNameDictionary(record);
            AppendToDateOfBirthDictionary(record);
        }

        private static void AppendToFirstNameDictionary(FileCabinetRecord record)
        {
            if (!firstNameDictionary.ContainsKey(record.FirstName))
            {
                firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord> { record });
            }
            else
            {
                firstNameDictionary[record.FirstName].Add(record);
            }
        }

        private static void AppendToLastNameDictionary(FileCabinetRecord record)
        {
            if (!lastNameDictionary.ContainsKey(record.LastName))
            {
                lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord> { record });
            }
            else
            {
                lastNameDictionary[record.LastName].Add(record);
            }
        }

        private static void AppendToDateOfBirthDictionary(FileCabinetRecord record)
        {
            if (!dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }
            else
            {
                dateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
        }

        private static void RemoveFromAllDictionaries(FileCabinetRecord record)
        {
            firstNameDictionary[record.FirstName].Remove(record);
            lastNameDictionary[record.LastName].Remove(record);
            dateOfBirthDictionary[record.DateOfBirth].Remove(record);
        }
        

        public static IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return firstNameDictionary[firstName].ToArray();
        }
        
        public static IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = new List<FileCabinetRecord>();
            foreach (var item in list)
            {
                if (item.LastName == lastName)
                {
                    records.Add(item);
                }
            }

            return records.ToArray();
        }
        
        public static IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var records = new List<FileCabinetRecord>();
            foreach (var item in list)
            {
                if (item.DateOfBirth == DateTime.ParseExact(dateOfBirth, "yyyy-MMM-dd", CultureInfo.InvariantCulture))
                {
                    records.Add(item);
                }
            }

            return records.ToArray();
        }
    }
}