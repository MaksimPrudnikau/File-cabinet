using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly IRecordValidator _validator;
        private readonly FileStream _outputFile;
        private int _stat;

        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            _outputFile = fileStream 
                          ?? new FileStream(FileCabinetConsts.FileSystemFileName, FileMode.Create);
            
            _validator = validator;
        }

        /// <summary>
        /// Create new record in base file with source parameters
        /// </summary>
        /// <param name="record">Source parameters to add</param>
        /// <returns>Id of created record</returns>
        /// <exception cref="ArgumentNullException">Parameters are null</exception>
        public int CreateRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var fileSystemRecord = new FilesystemRecord(record);
            
            fileSystemRecord.Serialize(_outputFile);

            return record.Id;
        }

        /// <summary>
        /// Edit already existing record with source
        /// </summary>
        /// <param name="record">Source for editing record</param>
        /// <exception cref="ArgumentNullException">Parameters are null</exception>
        /// <exception cref="ArgumentException">There is no record suitable for replacement</exception>
        public void EditRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Id * FilesystemRecord.Size > _outputFile.Length)
            {
                throw new ArgumentException($"Record with id = {record.Id} is not found");
            }

            _outputFile.Seek( (record.Id - 1) * FilesystemRecord.Size + 1, SeekOrigin.Begin);
            CreateRecord(record);
        }

        /// <summary>
        /// Read all records from base file convert to <see cref="FilesystemRecord"/> array
        /// </summary>
        /// <returns>Array of <see cref="FilesystemRecord"/></returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            try
            {
                return FileCabinetRecord.Deserialize(_outputFile);
            }
            catch (Exception e) when (e is ArgumentException or ArgumentNullException)
            {
                Console.Error.WriteLine(e.Message);
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Get count of records in base file
        /// </summary>
        /// <returns>The amount of all records in base file</returns>
        public int GetStat()
        {
            return _stat;
        }

        /// <summary>
        /// Read parameters from keyboard and validate according to source validator
        /// </summary>
        /// <param name="id">Id of read parameter. The default value indicates the numbering is sequential</param>
        /// <returns><see cref="FileCabinetRecord"/> object</returns>
        public FileCabinetRecord ReadParameters(int id = -1)
        {
            var record = new FileCabinetRecord
            {
                Id = id == -1 ? _stat + 1 : id,
                JobExperience = FileCabinetConsts.MinimalJobExperience,
                Wage = FileCabinetConsts.MinimalWage,
                Rank = FileCabinetConsts.MinimalRank
            };
            _stat++;
            
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
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Value to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with firstname equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            if (searchValue is null)
            {
                return Array.Empty<FileCabinetRecord>();
            }

            searchValue = string.Format(CultureInfo.InvariantCulture, "{0}", searchValue.PadRight(120, default));
            
            var records = new List<FileCabinetRecord>(GetRecords()).ToArray();
            return Array.FindAll(records, x => x.FirstName == searchValue);
        }

        /// <summary>
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Value to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with lastname equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            if (searchValue is null)
            {
                return Array.Empty<FileCabinetRecord>();
            }
            
            searchValue = string.Format(CultureInfo.InvariantCulture, "{0}", searchValue.PadRight(120, default));

            var records = new List<FileCabinetRecord>(GetRecords()).ToArray();
            return Array.FindAll(records, x => x.LastName == searchValue);
        }

        /// <summary>
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Date of birth in format dd/MM/yyyy</param>
        /// <returns><see cref="FileCabinetRecord"/> array with date of birth equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            if (searchValue is null)
            {
                return Array.Empty<FileCabinetRecord>();
            }
            
            var dateOfBirth = DateTime.ParseExact(searchValue, FileCabinetConsts.InputDateFormat,
                CultureInfo.InvariantCulture);
            
            var records = new List<FileCabinetRecord>(GetRecords()).ToArray();
            return Array.FindAll(records, x => x.DateOfBirth == dateOfBirth);
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}