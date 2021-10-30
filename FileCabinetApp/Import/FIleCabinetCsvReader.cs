using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Results;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Import
{
    public class FIleCabinetCsvReader
    {
        private readonly StreamReader _reader;
        private static readonly IRecordValidator Validator = new ValidationBuilder().CreateCustom();
        
        public FIleCabinetCsvReader(StreamReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Read all records from current base file written in csv
        /// </summary>
        /// <returns><see cref="IList{T}"/> of <see cref="FileCabinetRecord"/></returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();

            var readLines = _reader.ReadToEnd().Split(Environment.NewLine);

            for (var i = 1; i < readLines.Length; i++)
            {
                if (readLines[i].Length == 0) continue;
                
                try
                {
                    var record = DeserializeFromCsvLine(readLines[i]);
                    records.Add(record);
                }
                catch (ArgumentException e)
                {
                    Console.Error.WriteLine($"Record at line {i} Error: {e.Message}");
                }
            }

            return records;
        }

        /// <summary>
        /// Deserialize <see cref="FileCabinetRecord"/> from source line
        /// </summary>
        /// <param name="csvLine">Source line in csv format</param>
        /// <returns><see cref="FileCabinetRecord"/> object</returns>
        /// <exception cref="ArgumentException">The amount of parameters in line doesn't correspond
        /// to <see cref="FileCabinetRecord"/></exception>
        private static FileCabinetRecord DeserializeFromCsvLine(string csvLine)
        {
            const int parametersCount = 7;

            const int idIndex = 0;
            const int firstNameIndex = 1;
            const int lastNameIndex = 2;
            const int dateOfBirthIndex = 3;
            const int jobExperienceIndex = 4;
            const int wageIndex = 5;
            const int rankIndex = 6;

            var split = csvLine.Split(FileCabinetConsts.CsvDelimiter);

            if (split.Length != parametersCount)
            {
                throw new ArgumentException($"{csvLine} missing one parameter");
            }
            
            var record = new FileCabinetRecord
            {
                Id = Parse(split[idIndex], InputConverter.IdConverter),
                FirstName = Parse(split[firstNameIndex], InputConverter.NameConverter),
                LastName = Parse(split[lastNameIndex], InputConverter.NameConverter),
                DateOfBirth = Parse(split[dateOfBirthIndex], InputConverter.DateOfBirthConverter),
                JobExperience = Parse(split[jobExperienceIndex], InputConverter.JobExperienceConverter),
                Salary = Parse(split[wageIndex], InputConverter.WageConverter),
                Rank = Parse(split[rankIndex], InputConverter.RankConverter)
            };

            Validator.Validate(record);
            return record;
        }

        /// <summary>
        /// Parse source string to suitable <see cref="FileCabinetRecord"/> parameter applying certain rules
        /// to convert and validate
        /// </summary>
        /// <param name="source">Source string to parse</param>
        /// <param name="converter">Source converter with convert methods</param>
        /// <typeparam name="T">The output parsing type</typeparam>
        /// <returns>Object parsed to output type and satisfied validator's methods</returns>
        /// <exception cref="ArgumentException">Cannot convert the source value
        /// or it's doesnt satisfy validation rules </exception>
        private static T Parse<T>(string source, Func<string, ConversionResult<T>> converter)
        {
            var conversionResult = converter(source);

            if (!conversionResult.Parsed)
            {
                throw new ArgumentException($"{source} Error: wrong conversion");
            }

            return conversionResult.Result;
        }
    }
}