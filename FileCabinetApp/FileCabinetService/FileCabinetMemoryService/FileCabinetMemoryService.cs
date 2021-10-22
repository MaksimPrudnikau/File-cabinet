using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private static IRecordValidator _validator;

        private static readonly Dictionary<int, FileCabinetRecord> Records = new ();
        private static readonly Statistic Stat = new();

        private static readonly Dictionary<string, List<FileCabinetRecord>> FirstNameDictionary = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> LastNameDictionary = new();
        private static readonly Dictionary<DateTime, List<FileCabinetRecord>> DateOfBirthDictionary = new();

        private static bool _isCustomService;
        private static int _maxId;

        public FileCabinetMemoryService(IRecordValidator validator, bool isCustom = false)
        {
            _validator = validator;
            _isCustomService = isCustom;
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

            if (record.Id > _maxId)
            {
                _maxId = record.Id;
            }

            Records.Add(record.Id, record);

            AppendToAllDictionaries(Records[record.Id]);
            Stat.Count++;

            return record.Id;
        }

        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return Records.Values;
        }

        /// <summary>
        /// Returns number of records that the service stores
        /// </summary>
        /// <value>An ordinal number of the last record</value>
        public Statistic GetStat()
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
                Id = id == -1 ? _maxId + 1 : id,
            };

            Console.Write(EnglishSource.first_name);
            record.FirstName = ReadInput(InputConverter.NameConverter);
            
            Console.Write(EnglishSource.last_name);
            record.LastName = ReadInput(InputConverter.NameConverter);
            
            Console.Write(EnglishSource.date_of_birth);
            record.DateOfBirth = ReadInput(InputConverter.DateOfBirthConverter);
            
            if (_isCustomService)
            {
                Console.Write(EnglishSource.job_experience);
                record.JobExperience = ReadInput(InputConverter.JobExperienceConverter);
                
                Console.Write(EnglishSource.wage);
                record.Salary = ReadInput(InputConverter.WageConverter);
                
                Console.Write(EnglishSource.rank);
                record.Rank = ReadInput(InputConverter.RankConverter);
            }

            _validator.Validate(record);
            return record;
        }

        /// <summary>
        /// Read data from keyboard, convert it by source converter and validate with source validator
        /// </summary>
        /// <typeparam name="T">Type of read data</typeparam>
        /// <param name="converter">Source converter</param>
        /// <returns>Correct input object</returns>
        private static T ReadInput<T>(Func<string, ConversionResult<T>> converter)
        {
            do
            {
                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (conversionResult.Parsed)
                {
                    return conversionResult.Result;
                }

                Console.WriteLine(EnglishSource.ReadInput_Conversion_failed, conversionResult.StringRepresentation);
            }
            while (true);
        }
        
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
            
            RemoveFromAllDictionaries(Records[record.Id]);

            Records[record.Id] = record;
            
            AppendToAllDictionaries(Records[record.Id]);
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

        private static IEnumerable<FileCabinetRecord> Find(string searchValue, SearchValue attribute)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return Array.Empty<FileCabinetRecord>();
            }

            try
            {
                return attribute switch
                {
                    SearchValue.FirstName => FirstNameDictionary[searchValue],
                    SearchValue.LastName => LastNameDictionary[searchValue],
                    SearchValue.DateOfBirth => DateOfBirthDictionary[
                        DateTime.ParseExact(searchValue, FileCabinetConsts.InputDateFormat, CultureInfo.InvariantCulture)],
                    _ => throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null)
                };
            }
            catch (KeyNotFoundException)
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable first name
        /// </summary>
        /// <param name="searchValue">First name to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable first name</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            return Find(searchValue, SearchValue.FirstName);
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable last name
        /// </summary>
        /// <param name="searchValue">Last name to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable last name</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            return Find(searchValue, SearchValue.LastName);
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable date of birth
        /// </summary>
        /// <param name="searchValue">Date of birth to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable date of birth</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            return Find(searchValue, SearchValue.DateOfBirth);
        }

        /// <summary>
        /// Create <see cref="FileCabinetServiceSnapshot"/> object with current record array
        /// </summary>
        /// <returns><see cref="FileCabinetServiceSnapshot"/> object</returns>
        public static FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(Records.Values);
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }
            
            foreach (var item in snapshot.Records)
            {
                if (!Records.ContainsKey(item.Id))
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
            if (!Records.ContainsKey(id))
            {
                throw new ArgumentException($"Record #{id} doesn't exist");
            }

            RemoveFromAllDictionaries(Records[id]);
            Stat.Count--;
            Records.Remove(id);
        }

        public void Purge()
        {
        }
    }
}