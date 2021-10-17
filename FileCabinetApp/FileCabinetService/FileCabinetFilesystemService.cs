using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private readonly IRecordValidator _validator;
        private FileStream _outputFile;
        private readonly Statistic _stat = new();
        private bool _disposed;

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

            _stat.Count++;

            return record.Id;
        }

        /// <summary>
        /// Move the base's file cursor to record with source id 
        /// </summary>
        /// <param name="id">Source record's id</param>
        /// <exception cref="ArgumentException">Record with input id is not found</exception>
        private void MoveCursorToRecord(int id)
        {
            if (id * FilesystemRecord.Size > _outputFile.Length)
            {
                throw new ArgumentException($"Record with id = {id} is not found");
            }
            
            _outputFile.Seek((id - 1) * FilesystemRecord.Size, SeekOrigin.Begin);
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

            MoveCursorToRecord(record.Id);
            CreateRecord(record);
            _stat.Count--;
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
        public Statistic GetStat()
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
                Id = id == -1 ? _stat.Count + 1 : id,
                JobExperience = FileCabinetConsts.MinimalJobExperience,
                Salary = FileCabinetConsts.MinimalSalary,
                Rank = FileCabinetConsts.Grades[0]
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
            record.Salary = ReadInput(InputConverter.WageConverter, _validator.WageValidator);
                
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
        /// Find record with source attribute equals searchValue
        /// </summary>
        /// <param name="searchValue">Value to search</param>
        /// <param name="attribute">Attribute to comparing</param>
        /// <returns><see cref="FileCabinetRecord"/> array of records with suitable value of attribute</returns>
        /// <exception cref="ArgumentOutOfRangeException">Search value is null</exception>
        private IEnumerable<FileCabinetRecord> Find(string searchValue, SearchValue attribute)
        {
            if (searchValue is null)
            {
                return Array.Empty<FileCabinetRecord>();
            }
            
            var records = new List<FileCabinetRecord>();

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

        /// <summary>
        /// Update current records with snapshot. If record is exist it will be overwritten, else append to end
        /// </summary>
        /// <param name="snapshot">Source snapshot</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot?.Records is null || snapshot.Records.Count == 0)
            {
                return;
            }

            var records = new List<FileCabinetRecord>(snapshot.Records);
            
            if (_outputFile.Length == 0)
            {
                foreach (var item in records)
                {
                    CreateRecord(item);
                }
                
                return;
            }

            _outputFile.Seek(0, SeekOrigin.Begin);
            
            while (_outputFile.Position < _outputFile.Length)
            {
                var read = FileCabinetRecord.ReadRecord(_outputFile).ToFileCabinetRecord();
                _outputFile.Position -= FilesystemRecord.Size;

                foreach (var item in records)
                {
                    if (item.Id == read.Id)
                    {
                        CreateRecord(item);
                        records.Remove(item);

                        _stat.Count--;
                        _outputFile.Position -= FilesystemRecord.Size;
                        break;
                    }
                }

                _outputFile.Position += FilesystemRecord.Size;
            }

            foreach (var item in records)
            {
                CreateRecord(item);
            }
        }

        /// <summary>
        /// Marks the record with input id as deleted
        /// </summary>
        /// <param name="id">Source record's id</param>
        public void Remove(int id)
        {
            _outputFile.Seek(0, SeekOrigin.Begin);
            
            while (_outputFile.Position < _outputFile.Length)
            {
                var read = FileCabinetRecord.ReadRecord(_outputFile).ToFileCabinetRecord();
                _outputFile.Position -= FilesystemRecord.Size;

                if (id == read.Id)
                {
                    var buffer = new byte[FilesystemRecord.Size];
                    
                    _outputFile.Read(buffer);
                    _outputFile.Position -= FilesystemRecord.Size;

                    var deletedFlag = buffer[FilesystemRecord.IsDeletedIndex];
                    
                    buffer[FilesystemRecord.IsDeletedIndex] = deletedFlag switch
                    {
                        0 => 1,
                        1 => throw new ArgumentException($"Record #{id} is already deleted"),
                        _ => throw new ArgumentException($"Error isDeleted flat ({deletedFlag})")
                    };

                    _outputFile.Write(buffer);

                    _stat.Deleted++;
                    _stat.Count--;
                    return;
                }

                _outputFile.Position += FilesystemRecord.Size;
            }

            throw new ArgumentException($"Record #{id} is not exist");
        }

        /// <summary>
        /// Delete all records marked as delete from current file by creating a new one 
        /// </summary>
        public void Purge()
        {
            var path = _outputFile.Name;

            var snapshot = FileCabinetServiceSnapshot.CopyAndDelete(_outputFile, this);

            _outputFile = new FileStream(path, FileMode.CreateNew);
            _stat.Count = _stat.Deleted = 0;
            
            foreach (var item in snapshot.Records)
            {
                CreateRecord(item);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            { 
                if(disposing)
                {
                    _outputFile.Dispose();
                }
                
                _disposed = true;
            }
        }
    }
}