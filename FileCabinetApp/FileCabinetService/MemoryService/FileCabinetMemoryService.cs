using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService.Iterators;
using FileCabinetApp.Results;
using FileCabinetApp.Validators;

namespace FileCabinetApp.FileCabinetService.MemoryService
{
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private static IRecordValidator _validator;
        private static readonly Statistic Stat = new();

        private readonly MemoryServiceDictionaries _dictionaries = new();

        private static bool _isCustomService;
        private static int _maxId;

        public FileCabinetMemoryService(IRecordValidator validator, bool isCustom = false)
        {
            _validator = validator;
            _isCustomService = isCustom;
        }

        /// <summary>
        /// The method create new record from source record and return its id
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
            
            _dictionaries.Add(record);
            
            Stat.Count++;

            return record.Id;
        }

        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var item in _dictionaries.Records.Values)
            {
                yield return item;
            }
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
                record.Salary = ReadInput(InputConverter.SalaryConverter);
                
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
        public int EditRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            _dictionaries.Remove(record);
            _dictionaries.Add(record);

            return record.Id;
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable first name
        /// </summary>
        /// <param name="searchValue">First name to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable first name</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            return new MemoryIterator(_dictionaries.FirstNames[searchValue]);
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable last name
        /// </summary>
        /// <param name="searchValue">Last name to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable last name</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            return new MemoryIterator(_dictionaries.LastNames[searchValue]);
        }

        /// <summary>
        /// Find all occurrences of <see cref="FileCabinetRecord"/> with suitable date of birth
        /// </summary>
        /// <param name="searchValue">Date of birth to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with suitable date of birth</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            var dateOfBirth = DateTime.ParseExact(searchValue, FileCabinetConsts.InputDateFormat,
                CultureInfo.InvariantCulture);
            
            return new MemoryIterator(_dictionaries.DateOfBirths[dateOfBirth]);
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }
            
            foreach (var item in snapshot.Records)
            {
                if (!_dictionaries.Records.ContainsKey(item.Id))
                {
                    CreateRecord(item);
                }
                else
                {
                    EditRecord(item);
                }
            }
        }

        public IEnumerable<int> Delete(SearchValue searchValue)
        {
            var deletedRecordId = new List<int>(_dictionaries.Remove(searchValue));
            Stat.Count-= deletedRecordId.Count;
            return deletedRecordId;
        }

        public void Purge()
        {
        }

        public void Insert(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (_dictionaries.Records.ContainsKey(record.Id))
            {
                throw new ArgumentException($"Record with id = '{record.Id}' is already exist");
            }

            if (record.Id == default)
            {
                record.Id = _maxId + 1;
            }

            CreateRecord(record);
        }

        public void Update(IEnumerable<SearchValue> values, IEnumerable<SearchValue> @where)
        {
            throw new NotImplementedException();
        }
    }
}