using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        /// <param name="parameters">Source parameters to add</param>
        /// <returns>Id of created record</returns>
        /// <exception cref="ArgumentNullException">Parameters are null</exception>
        public int CreateRecord(Parameter parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var fileSystemRecord = new FilesystemRecord(parameters);

            _outputFile.Write(fileSystemRecord.GetStatus(), 0, fileSystemRecord.GetStatus().Length);

            _outputFile.Write(fileSystemRecord.GetId(), 0, fileSystemRecord.GetId().Length);

            _outputFile.Write(fileSystemRecord.GetFirstName(), 0, fileSystemRecord.GetFirstName().Length);

            _outputFile.Write(fileSystemRecord.GetLastName(), 0, fileSystemRecord.GetLastName().Length);

            _outputFile.Write(fileSystemRecord.GetYear(), 0, fileSystemRecord.GetYear().Length);

            _outputFile.Write(fileSystemRecord.GetMonth(), 0, fileSystemRecord.GetMonth().Length);

            _outputFile.Write(fileSystemRecord.GetDay(), 0, fileSystemRecord.GetDay().Length);

            _outputFile.Write(fileSystemRecord.GetJobExperience(), 0, fileSystemRecord.GetJobExperience().Length);

            _outputFile.Write(fileSystemRecord.GetWage(), 0, fileSystemRecord.GetWage().Length);

            _outputFile.Write(fileSystemRecord.GetRank(), 0, fileSystemRecord.GetRank().Length);

            _outputFile.Flush();

            return parameters.Id;
        }

        public void EditRecord(Parameter parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Id * FilesystemRecord.Size > _outputFile.Length)
            {
                throw new ArgumentException($"Record with id = {parameters.Id} is not found");
            }

            _outputFile.Seek( (parameters.Id - 1) * FilesystemRecord.Size + 1, SeekOrigin.Begin);
            CreateRecord(parameters);
        }

        /// <summary>
        /// Read all records from base file convert to <see cref="FilesystemRecord"/> array
        /// </summary>
        /// <returns>Array of <see cref="FilesystemRecord"/></returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var array = new List<FileCabinetRecord>();

            var buffer = new byte[FilesystemRecord.Size];
            
            var currentIndex = 0;
            _outputFile.Seek(currentIndex, SeekOrigin.Begin);
            
            while (currentIndex < _outputFile.Length)
            {
                _outputFile.Read(buffer, 0, buffer.Length);
                var readRecord = new FilesystemRecord(buffer);
                
                array.Add(new FileCabinetRecord
                {
                    Id = BitConverter.ToInt32(readRecord.GetId()),
                    FirstName = Encoding.UTF8.GetString(readRecord.GetFirstName()),
                    LastName = Encoding.UTF8.GetString(readRecord.GetLastName()),
                    DateOfBirth = new DateTime(
                        BitConverter.ToInt32(readRecord.GetYear()),
                        BitConverter.ToInt32(readRecord.GetMonth()),
                        BitConverter.ToInt32(readRecord.GetDay())
                    ),
                    JobExperience = BitConverter.ToInt16(readRecord.GetJobExperience()),
                    Wage = new decimal(BitConverter.ToDouble(readRecord.GetWage())),
                    Rank = Encoding.UTF8.GetString(readRecord.GetRank())[0]
                });
                
                
                
                currentIndex += FilesystemRecord.Size + 1;
                _outputFile.Seek(1, SeekOrigin.Current);
            }

            return array.ToArray();
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
        public Parameter ReadParameters(int id = -1)
        {
            var record = new Parameter
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
        /// Prints all records to console
        /// </summary>
        public void PrintRecords()
        {
            foreach (var item in GetRecords())
            {
                item.Print();
            }
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            throw new NotImplementedException();
        }
    }
}