using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private static IRecordValidator _validator;

        private static Dictionary<int, FileCabinetRecord> _records = new ();

        private static readonly Dictionary<string, List<FileCabinetRecord>> FirstNameDictionary = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> LastNameDictionary = new();
        private static readonly Dictionary<DateTime, List<FileCabinetRecord>> DateOfBirthDictionary = new();

        public FileCabinetMemoryService(IRecordValidator validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// The method create new record from input data and return its id
        /// </summary>
        /// <returns>An id of current record</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            _records.Add(record.Id, record);

            AppendToAllDictionaries(_records[record.Id]);
            
            return record.Id;
        }

        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return _records.Values;
        }

        public int GetStat()
        {
            return Stat;
        }

        /// <summary>
        /// Read parameters from keyboard and parse it to <see cref="FileCabinetRecord"/> object
        /// </summary>
        /// <param name="id">Source id of read parameter</param>
        /// <returns><see cref="FileCabinetServiceSnapshot"/> object equivalent for read parameters</returns>
        public FileCabinetRecord ReadParameters(int id = -1)
        {   
            var record = new FileCabinetRecord
            {
                Id = id == -1 ? Stat + 1 : id,
            };

            Console.Write(EnglishSource.first_name);
            record.FirstName = ReadInput(InputConverter.NameConverter, _validator.NameValidator);
            
            Console.Write(EnglishSource.last_name);
            record.LastName = ReadInput(InputConverter.NameConverter, _validator.NameValidator);
            
            Console.Write(EnglishSource.date_of_birth);
            record.DateOfBirth = ReadInput(InputConverter.DateOfBirthConverter, _validator.DateOfBirthValidator);

            if (_validator is not CustomValidator)
            {
                return record;
            }
            
            Console.Write(EnglishSource.job_experience);
            record.JobExperience = ReadInput(InputConverter.JobExperienceConverter,
                _validator.JobExperienceValidator);
                
            Console.Write(EnglishSource.wage);
            record.Wage = ReadInput(InputConverter.WageConverter, _validator.WageValidator);
                
            Console.Write(EnglishSource.rank);
            record.Rank = ReadInput(InputConverter.RankConverter, _validator.RankValidator);

            return record;
        }

        /// <summary>
        /// Read data from keyboard, convert it by source converter and validate with source validator
        /// </summary>
        /// <typeparam name="T">Type of read data</typeparam>
        /// <param name="converter">Source converter</param>
        /// <param name="validator">Source validator</param>
        /// <returns>Correct input object</returns>
        private static T ReadInput<T>(Func<string, ConversionResult<T>> converter, Func<T, ValidationResult> validator)
        {
            do
            {
                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Parsed)
                {
                    Console.WriteLine(EnglishSource.ReadInput_Conversion_failed, conversionResult.StringRepresentation);
                    continue;
                }

                var value = conversionResult.Result;

                var validationResult = validator(value);
                if (validationResult.Parsed)
                {
                    return value;
                }
                
                Console.WriteLine(EnglishSource.Validation_failed, validationResult.StringRepresentation);
            }
            while (true);
        }
        
        /// <summary>
        /// Returns number of records that the service stores
        /// </summary>
        /// <value>An ordinal number of the last record</value>
        private static int Stat => _records.Count;
        
        /// <summary>
        /// Edit record with the source one
        /// </summary>
        /// <param name="record">Parameter contains new data</param>
        public void EditRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            RemoveFromAllDictionaries(_records[record.Id]);

            _records[record.Id] = record;
            
            AppendToAllDictionaries(_records[record.Id]);
        }

        /// <summary>
        /// Append record to all dictionaries
        /// </summary>
        /// <param name="record">Source record</param>
        private static void AppendToAllDictionaries(FileCabinetRecord record)
        {
            AppendToFirstNameDictionary(record);
            AppendToLastNameDictionary(record);
            AppendToDateOfBirthDictionary(record);
        }

        /// <summary>
        /// Append record to FirstNameDictionary
        /// </summary>
        /// <param name="record">Source record</param>
        private static void AppendToFirstNameDictionary(FileCabinetRecord record)
        {
            if (!FirstNameDictionary.ContainsKey(record.FirstName))
            {
                FirstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord> { record });
            }
            else
            {
                FirstNameDictionary[record.FirstName].Add(record);
            }
        }

        /// <summary>
        /// Append record to LastNameDictionary
        /// </summary>
        /// <param name="record">Source record</param>
        private static void AppendToLastNameDictionary(FileCabinetRecord record)
        {
            if (!LastNameDictionary.ContainsKey(record.LastName))
            {
                LastNameDictionary.Add(record.LastName, new List<FileCabinetRecord> { record });
            }
            else
            {
                LastNameDictionary[record.LastName].Add(record);
            }
        }

        /// <summary>
        /// Append record to DateOfBirthDictionary
        /// </summary>
        /// <param name="record">Source record</param>
        private static void AppendToDateOfBirthDictionary(FileCabinetRecord record)
        {
            if (!DateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                DateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }
            else
            {
                DateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
        }

        /// <summary>
        /// Remove record from all dictionaries
        /// </summary>
        /// <param name="record">Source record to remove</param>
        private static void RemoveFromAllDictionaries(FileCabinetRecord record)
        {
            FirstNameDictionary[record.FirstName].Remove(record);

            LastNameDictionary[record.LastName].Remove(record);

            DateOfBirthDictionary[record.DateOfBirth].Remove(record);
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable first name
        /// </summary>
        /// <param name="searchValue">First name to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable first name</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            try
            {
                return FirstNameDictionary[searchValue];
            }
            catch (KeyNotFoundException)
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable last name
        /// </summary>
        /// <param name="searchValue">Last name to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable last name</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            try
            {
                return LastNameDictionary[searchValue];
            }
            catch (KeyNotFoundException)
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable date of birth
        /// </summary>
        /// <param name="searchValue">Date of birth to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable date of birth</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            var dateOfBirth = DateTime.ParseExact(searchValue, FileCabinetConsts.InputDateFormat, CultureInfo.InvariantCulture);

            if (string.IsNullOrEmpty(searchValue))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            try
            {
                return DateOfBirthDictionary[dateOfBirth];
            }
            catch (KeyNotFoundException)
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Create <see cref="FileCabinetServiceSnapshot"/> object with current record array
        /// </summary>
        /// <returns><see cref="FileCabinetServiceSnapshot"/> object</returns>
        public static FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(_records.Values);
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }
            
            foreach (var item in snapshot.Records)
            {
                if (!_records.ContainsKey(item.Id))
                {
                    CreateRecord(item);
                }
                else
                {
                    EditRecord(item);
                }
            }
        }

        public void Remove(int id)
        {
            if (!_records.ContainsKey(id))
            {
                throw new ArgumentException($"Record #{id} doesn't exist");
            }

            RemoveFromAllDictionaries(_records[id]);
            _records.Remove(id);
        }
    }
}