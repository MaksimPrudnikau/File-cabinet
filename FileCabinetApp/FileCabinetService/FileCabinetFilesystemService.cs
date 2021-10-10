﻿using System;
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

        private void MoveCursorToRecord(int id) => _outputFile.Seek( (id - 1) * FilesystemRecord.Size, SeekOrigin.Begin);
        
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

            MoveCursorToRecord(record.Id);
            CreateRecord(record);
        }

        /// <summary>
        /// Read all records from base file convert to <see cref="FilesystemRecord"/> array
        /// </summary>
        /// <returns>Array of <see cref="FilesystemRecord"/></returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var record = new FileCabinetRecord();
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
                Rank = FileCabinetConsts.Grades[0]
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

        private IEnumerable<FileCabinetRecord> Find(string searchValue, SearchValue attribute)
        {
            if (searchValue is null)
            {
                return Array.Empty<FileCabinetRecord>();
            }
            
            var records = new List<FileCabinetRecord>();
            var record = new FileCabinetRecord();
            
            var currentIndex = 0;
            _outputFile.Seek(currentIndex, SeekOrigin.Begin);
            
            while (currentIndex < _outputFile.Length)
            {
                var read = FileCabinetRecord.ReadRecord(_outputFile);
                if (!read.IsDeleted)
                {
                    var value = attribute switch
                    {
                        SearchValue.FirstName => read.ToFileCabinetRecord().FirstName,
                        SearchValue.LastName => read.ToFileCabinetRecord().LastName,
                        SearchValue.DateOfBirth => read.ToFileCabinetRecord().DateOfBirth
                            .ToString(FileCabinetConsts.InputDateFormat, CultureInfo.InvariantCulture),
                        _ => throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null)
                    };

                    if (string.Equals(value, searchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        records.Add(read.ToFileCabinetRecord());
                    }
                }

                currentIndex += FilesystemRecord.Size;
                
            }

            return records;
        }

        /// <summary>
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Value to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with firstname equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            try
            {
                return Find(searchValue, SearchValue.FirstName);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception.Message);
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Value to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with lastname equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            try
            {
                return Find(searchValue, SearchValue.LastName);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception.Message);
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Date of birth in format dd/MM/yyyy</param>
        /// <returns><see cref="FileCabinetRecord"/> array with date of birth equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            try
            {
                return Find(searchValue, SearchValue.DateOfBirth);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception.Message);
                return Array.Empty<FileCabinetRecord>();
            }
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot?.Records is null || snapshot.Records.Count == 0)
            {
                return;
            }

            var record = new FileCabinetRecord();
            var records = new List<FileCabinetRecord>(snapshot.Records);
            
            if (_outputFile.Length == 0)
            {
                foreach (var item in records)
                {
                    CreateRecord(item);
                }
                
                return;
            }

            var currentIndex = 0;
            _outputFile.Seek(currentIndex, SeekOrigin.Begin);
            
            while (currentIndex < _outputFile.Length)
            {
                var read = FileCabinetRecord.ReadRecord(_outputFile).ToFileCabinetRecord();

                foreach (var item in records)
                {
                    if (item.Id == read.Id)
                    {
                        EditRecord(item);
                        records.Remove(item);
                        break;
                    }
                }

                currentIndex += FilesystemRecord.Size;
                
            }

            foreach (var item in records)
            {
                CreateRecord(item);
            }
        }

        public void Remove(int id)
        {
            var buffer = new byte[FilesystemRecord.Size];
            
            MoveCursorToRecord(id);
            
            _outputFile.Read(buffer);
            
            buffer[FilesystemRecord.IsDeletedIndex] = 1;
            
            MoveCursorToRecord(id);
            
            _outputFile.Write(buffer);
        }

        public void Purge()
        {
            throw new NotImplementedException();
        }
    }
}