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
        
        private readonly bool _isCustomService;
        private int _maxId;

        private FileSystemWriter _writer;
        private FileSystemReader _reader;

        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator, bool isCustom = false)
        {
            _outputFile = fileStream 
                          ?? new FileStream(FileCabinetConsts.FileSystemFileName, FileMode.Create);
            
            _validator = validator;
            
            _isCustomService = isCustom;

            _writer = new FileSystemWriter(this, _outputFile);
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
        /// Read parameters from keyboard and validate according to source validator
        /// </summary>
        /// <param name="id">Id of read parameter. The default value indicates the numbering is sequential</param>
        /// <returns><see cref="FileCabinetRecord"/> object</returns>
        public FileCabinetRecord ReadParameters(int id = -1)
        {
            var record = new FileCabinetMemoryService(_validator, _isCustomService).ReadParameters();
            record.Id = _maxId + 1;
            return record;
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

            _outputFile.Seek(0, SeekOrigin.Begin);
            while (_outputFile.Position < _outputFile.Length)
            {
                var filesystemRecord = _reader.ReadRecord();
                if (!filesystemRecord.IsDeleted)
                {
                    var record = filesystemRecord.ToFileCabinetRecord();

                    var value = ExtractValueByAttribute(record, attribute);

                    if (string.Equals(value, searchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        records.Add(record);
                    }
                }
            }

            return records;
        }

        private static string ExtractValueByAttribute(FileCabinetRecord record, SearchValue attribute)
        {
            return attribute switch
            {
                SearchValue.FirstName => record.FirstName,
                SearchValue.LastName => record.LastName,
                SearchValue.DateOfBirth => record.DateOfBirth
                    .ToString(FileCabinetConsts.InputDateFormat, CultureInfo.InvariantCulture),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null)
            };
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
            _writer = new FileSystemWriter(this, _outputFile);
            _reader = new FileSystemReader(_outputFile);
            
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