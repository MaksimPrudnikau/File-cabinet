using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService.Iterators;
using FileCabinetApp.Results;
using FileCabinetApp.Validators;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private Index _recordsIndex = new();
        private readonly IRecordValidator _validator;
        private FileStream _outputFile;
        private readonly Statistic _stat = new();
        private bool _disposed;
        private readonly bool _isCustomService;
        private int _maxId;
        private FileSystemWriter _writer;
        private FileSystemReader _reader;

        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator, bool isCustom = false)
        {
            _outputFile = fileStream 
                          ?? new FileStream(FileCabinetConsts.FileSystemFileName, FileMode.Create);
            
            _validator = validator ?? new ValidationBuilder().CreateDefault();
            
            _isCustomService = isCustom;

            _writer = new FileSystemWriter(this, _outputFile, _recordsIndex);
            _reader = new FileSystemReader(_outputFile);
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
            
            if (record.Id > _maxId)
            {
                _maxId = record.Id;
            }
            
            _recordsIndex.Add(record, _outputFile.Position);

            var fileSystemRecord = new FilesystemRecord(record);
            
            fileSystemRecord.Serialize(_outputFile);

            _stat.Count++;

            return record.Id;
        }

        /// <summary>
        /// Overwriting existing record by the source
        /// </summary>
        /// <param name="record">Source record</param>
        private void Rewrite(FileCabinetRecord record)
        {
            CreateRecord(record);
            _stat.Count--;
        }
        
        /// <summary>
        /// Edit already existing record with source
        /// </summary>
        /// <param name="record">Source for editing record</param>
        /// <exception cref="ArgumentNullException">Parameters are null</exception>
        /// <exception cref="ArgumentException">There is no record suitable for replacement</exception>
        public int EditRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            _outputFile.Seek(0, SeekOrigin.Begin);
            while (_outputFile.Position < _outputFile.Length)
            {
                var read = _reader.ReadAndMoveCursorBack();
                if (read.Id == record.Id)
                {
                    Rewrite(record);
                    return record.Id;
                }
            }

            throw new ArgumentException($"Record #{record.Id} is not found");
        }

        /// <summary>
        /// Read all records from base file convert to <see cref="FilesystemRecord"/> array
        /// </summary>
        /// <returns>Array of <see cref="FilesystemRecord"/></returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            try
            {
                return _reader.Deserialize();
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
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Value to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with firstname equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            var records = new List<long>();
            try
            {
                records = new List<long>(_recordsIndex.FirstNames[searchValue]);
                return new FilesystemIterator(_reader, records);
            }
            catch (KeyNotFoundException)
            {
                return new FilesystemIterator(_reader, records);
            }
        }

        /// <summary>
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Value to search</param>
        /// <returns><see cref="FileCabinetRecord"/> array with lastname equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            var records = new List<long>();
            try
            {
                records = new List<long>(_recordsIndex.LastNames[searchValue]);
                return new FilesystemIterator(_reader, records);
            }
            catch (KeyNotFoundException)
            {
                return new FilesystemIterator(_reader, records);
            }
        }

        /// <summary>
        /// Find all occurrences of searchValue in records of current data base file
        /// </summary>
        /// <param name="searchValue">Date of birth in format dd/MM/yyyy</param>
        /// <returns><see cref="FileCabinetRecord"/> array with date of birth equals searchValue</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            var records = new List<long>();
            
            try
            {
                var dateOfBirth = DateTime.ParseExact(searchValue, FileCabinetConsts.InputDateFormat,
                    CultureInfo.InvariantCulture);
                
                records = new List<long>(_recordsIndex.DateOfBirths[dateOfBirth]);
                return new FilesystemIterator(_reader, records);
            }
            catch (SystemException exception) 
                when (exception is ArgumentException or FormatException or KeyNotFoundException)
            {
                return new FilesystemIterator(_reader, records);
            }
            catch (KeyNotFoundException)
            {
                Console.Error.WriteLine(EnglishSource.FindBy_is_not_presented_in_current_database, searchValue);
                return new FilesystemIterator(_reader, records);
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

            _outputFile.Seek(0, SeekOrigin.Begin);
            while (_outputFile.Position < _outputFile.Length)
            {
                var read = _reader.ReadAndMoveCursorBack();
                
                _writer.RewriteWithAny(read, records);

                _stat.Count--;
                _outputFile.Position += FilesystemRecord.Size;
            }
            
            _writer.AppendRange(records);
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
                var read = _reader.ReadAndMoveCursorBack();

                if (id == read.Id)
                {
                    _writer.MarkAsDeleted(id);
                    
                    _stat.Deleted++;
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
            _writer = new FileSystemWriter(this, _outputFile, _recordsIndex);
            _reader = new FileSystemReader(_outputFile);
            _recordsIndex = new Index();
            
            _writer.AppendRange(snapshot.Records);
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