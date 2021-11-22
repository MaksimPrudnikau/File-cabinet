using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService.FileSystemService;
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

        /// <summary>
        /// Read all the records from current service
        /// </summary>
        /// <returns>An array of <see cref="FileCabinetRecord"/></returns>
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
            var input = Console.ReadLine();
            var conversionResult = converter(input);

            if (!conversionResult.Parsed)
            {
                Console.WriteLine(EnglishSource.ReadInput_Conversion_failed, conversionResult.StringRepresentation);
            }

            return conversionResult.Result;
        }

        /// <summary>
        /// Add the records from snapshot to current service. Records from snapshot that are already exist will be
        /// edited. The others will be added
        /// </summary>
        /// <param name="snapshot"></param>
        /// <exception cref="ArgumentNullException">The source snapshot is null</exception>
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
                    _dictionaries.Edit(_dictionaries.Records[item.Id], item);
                }
            }
        }

        /// <summary>
        /// Delete all records satisfy the source value
        /// </summary>
        /// <param name="searchValue">Source value to search</param>
        /// <returns>Identifications of deleted records</returns>
        public IEnumerable<int> Delete(SearchValue searchValue)
        {
            var deletedRecordId = new List<int>(_dictionaries.Remove(searchValue));
            Stat.Count-= deletedRecordId.Count;
            return deletedRecordId.Count > 0
                ? deletedRecordId
                : null;
        }

        public void Purge()
        {
        }

        /// <summary>
        /// Insert the source record to service by it's id
        /// </summary>
        /// <param name="record">The source record to insert</param>
        /// <exception cref="ArgumentNullException">The source record is null</exception>
        /// <exception cref="ArgumentException">The record with id equals the source one is already exist</exception>
        public void Insert(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (_dictionaries.Records.ContainsKey(record.Id))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, EnglishSource.Record_with_id_is_already_exist, record.Id));
            }

            if (record.Id == default)
            {
                record.Id = _maxId + 1;
            }

            CreateRecord(record);
        }

        /// <summary>
        /// Find all records suitable records with source values
        /// </summary>
        /// <param name="values">The new values of records</param>
        /// <param name="where">An array of search values for which the record will be considered as suitable</param>
        /// <returns>Identifications of updated records</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<int> Update(IEnumerable<SearchValue> values, IList<SearchValue> where)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (where is null || where.Count == 0)
            {
                throw new ArgumentNullException(nameof(where));
            }
            
            var recordsToUpdate = new List<FileCabinetRecord>(_dictionaries.Find(where[0]));
            RemoveMismatch(recordsToUpdate, where);
            foreach (var item in recordsToUpdate)
            {
                var editRecord = item;
                foreach (var value in values)
                { 
                    editRecord = RecordHelper.EditByAttribute(item, value);
                }
                
                _dictionaries.Edit(item, editRecord);
                
                yield return editRecord.Id;
            }
        }

        /// <summary>
        /// Delete records from source array that satisfy match
        /// </summary>
        /// <param name="source">Source records</param>
        /// <param name="match">Source values to remove</param>
        /// <exception cref="ArgumentNullException">At least on of the source values is null</exception>
        private static void RemoveMismatch(IList<FileCabinetRecord> source, IEnumerable<SearchValue> match)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (var i = 0; i < source.Count;)
            {
                foreach (var attribute in match)
                {
                    if (!RecordHelper.Contains(source[i], attribute))
                    {
                        source.RemoveAt(i);
                        break;
                    }
                }

                i++;
            }
        }
    }
}