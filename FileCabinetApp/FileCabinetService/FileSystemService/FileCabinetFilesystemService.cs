using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Export;
using FileCabinetApp.Results;
using FileCabinetApp.Validators;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private readonly FileSystemServiceDictionaries _dictionaries = new();
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

            _writer = new FileSystemWriter(this, _outputFile, _dictionaries);
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
            
            _dictionaries.Add(record, _outputFile.Position);

            var fileSystemRecord = new FilesystemRecord(record);
            
            fileSystemRecord.Serialize(_outputFile);

            _stat.Count++;

            return record.Id;
        }

        /// <summary>
        /// Read all records from base file convert to <see cref="FilesystemRecord"/> array
        /// </summary>
        /// <returns>Array of <see cref="FilesystemRecord"/></returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
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

        public IEnumerable<int> Delete(SearchValue searchValue)
        {
            var positions = _dictionaries.GetPositionsByValue(searchValue);
            var deletedRecordId = new List<int>();
            foreach (var position in positions)
            {
                _reader.BaseFile.Seek(position, SeekOrigin.Begin);
                var read = _reader.ReadAndMoveCursorBack();

                if (!_writer.TryMarkAsDeleted(read.Id))
                {
                    continue;
                }
                
                deletedRecordId.Add(read.Id);
            }

            _stat.Count -= deletedRecordId.Count;
            _stat.Deleted += deletedRecordId.Count;
            return deletedRecordId;
        }

        /// <summary>
        /// Delete all records marked as delete from current file by creating a new one 
        /// </summary>
        public void Purge()
        {
            var path = _outputFile.Name;

            var snapshot = FileCabinetServiceSnapshot.CopyAndDelete(_outputFile, this);
            
            _outputFile = new FileStream(path, FileMode.CreateNew);
            _stat.Clear();
            _writer = new FileSystemWriter(this, _outputFile, _dictionaries);
            _reader = new FileSystemReader(_outputFile);
            _dictionaries.Clear();
            
            _writer.AppendRange(snapshot.Records);
        }

        public void Insert(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            if (_dictionaries.Id.ContainsKey(record.Id))
            {
                throw new ArgumentException($"Record with id = '{record.Id}' is already exist");
            }

            _dictionaries.Clear();
            _stat.Clear();
            var snapshot = new FileCabinetServiceSnapshot(this);
            _outputFile.Seek(0, SeekOrigin.Begin);
            
            var inserted = false;
            foreach (var item in snapshot.Records)
            {
                if (item.Id > record.Id && !inserted)
                {
                    CreateRecord(record);
                    inserted = true;
                }

                CreateRecord(item);
            }
        }

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

            var position = _dictionaries.Find(where[0]);
            
            foreach (var pos in position)
            {
                _reader.BaseFile.Seek(pos, SeekOrigin.Begin);
                var read = _reader.ReadAndMoveCursorBack();
                var recordContainsAllWheres = true;
                foreach (var value in where)
                {
                    if (value.Property is SearchValue.SearchProperty.Id)
                    {
                        throw new ArgumentException("Id cannot be updated");
                    }
                    
                    if (!RecordHelper.Contains(read, value))
                    {
                        recordContainsAllWheres = false;
                    }
                }

                if (!recordContainsAllWheres) continue;

                var editRecord = RecordHelper.Clone(read);
                foreach (var value in values)
                {
                    editRecord = RecordHelper.EditByAttribute(editRecord, value);
                }
                
                new FilesystemRecord(editRecord).Serialize(_reader.BaseFile);
                _dictionaries.Edit(read, editRecord, pos);
                
                yield return editRecord.Id;
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