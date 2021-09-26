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

            _outputFile.Write(fileSystemRecord.Status, 0, fileSystemRecord.Status.Length);

            _outputFile.Write(fileSystemRecord.Id, 0, fileSystemRecord.Id.Length);

            _outputFile.Write(fileSystemRecord.FirstName, 0, fileSystemRecord.FirstName.Length);

            _outputFile.Write(fileSystemRecord.LastName, 0, fileSystemRecord.LastName.Length);

            _outputFile.Write(fileSystemRecord.Year, 0, fileSystemRecord.Year.Length);

            _outputFile.Write(fileSystemRecord.Month, 0, fileSystemRecord.Month.Length);

            _outputFile.Write(fileSystemRecord.Day, 0, fileSystemRecord.Day.Length);

            _outputFile.Write(fileSystemRecord.JobExperience, 0, fileSystemRecord.JobExperience.Length);

            _outputFile.Write(fileSystemRecord.Wage, 0, fileSystemRecord.Wage.Length);

            _outputFile.Write(fileSystemRecord.Rank, 0, fileSystemRecord.Rank.Length);

            _outputFile.Flush();

            return parameters.Id;
        }

        public void EditRecord(Parameter parameters)
        {
            throw new System.NotImplementedException();
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
                    Id = BitConverter.ToInt32(readRecord.Id),
                    FirstName = Encoding.UTF8.GetString(readRecord.FirstName),
                    LastName = Encoding.UTF8.GetString(readRecord.LastName),
                    DateOfBirth = new DateTime(
                        BitConverter.ToInt32(readRecord.Year),
                        BitConverter.ToInt32(readRecord.Month),
                        BitConverter.ToInt32(readRecord.Day)
                    ),
                    JobExperience = BitConverter.ToInt16(readRecord.JobExperience),
                    Wage = new decimal(BitConverter.ToDouble(readRecord.Wage)),
                    Rank = Encoding.UTF8.GetString(readRecord.Rank)[0]
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
    }
}