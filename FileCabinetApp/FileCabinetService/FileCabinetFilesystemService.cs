using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private static IRecordValidator _validator;
        private readonly FileStream _outputFile = new(FileCabinetConsts.FileSystemFileName, FileMode.OpenOrCreate);
        private int _stat;

        public FileCabinetFilesystemService(IRecordValidator validator)
        {
            _validator = validator;
            _stat = 0;
        }

        public int CreateRecord(Parameter parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            var fileSystemRecord = new FilesystemRecord(parameters);
            var byteArray = BitConverter.GetBytes(fileSystemRecord.Status);
                _outputFile.Write(byteArray, 0, byteArray.Length);
            
                byteArray = BitConverter.GetBytes(fileSystemRecord.Id);
                _outputFile.Write(byteArray, 0, byteArray.Length);
            
                byteArray = ToBytes(fileSystemRecord.FirstName, 120);
                _outputFile.Write(byteArray, 0, byteArray.Length);

                byteArray = ToBytes(fileSystemRecord.LastName, 120);
                _outputFile.Write(byteArray,0, byteArray.Length);

                byteArray = BitConverter.GetBytes(fileSystemRecord.Year);
                _outputFile.Write(byteArray, 0, byteArray.Length);
            
                byteArray = BitConverter.GetBytes(fileSystemRecord.Month);
                _outputFile.Write(byteArray, 0, byteArray.Length);
            
                byteArray = BitConverter.GetBytes(fileSystemRecord.Day);
                _outputFile.Write(byteArray, 0, byteArray.Length);
            
                byteArray = BitConverter.GetBytes(fileSystemRecord.JobExperience);
                _outputFile.Write(byteArray, 0, byteArray.Length);
            
                byteArray = BitConverter.GetBytes(decimal.ToDouble(fileSystemRecord.Wage));
                _outputFile.Write(byteArray, 0, byteArray.Length);
            
                byteArray = BitConverter.GetBytes(fileSystemRecord.Rank);
                _outputFile.Write(byteArray, 0, byteArray.Length);
                
                _outputFile.Flush();

                return parameters.Id;
        }

        private static byte[] ToBytes(string value, int capacity)
        {
            var encoded = Encoding.Default.GetBytes(value);
            var byteArray = new byte[capacity];
            for (var i = 0; i < encoded.Length; i++)
            {
                byteArray[i] = encoded[i];
            }

            return byteArray;
        }

        public void EditRecord(Parameter parameters)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new System.NotImplementedException();
        }

        public int GetStat()
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Read parameters from keyboard and parse it to <see cref="Parameter"/> object
        /// </summary>
        /// <param name="id">Source id of read parameter</param>
        /// <returns><see cref="Parameter"/> object equivalent for read parameters</returns>
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

        public void PrintRecords()
        {
            throw new System.NotImplementedException();
        }
    }
}