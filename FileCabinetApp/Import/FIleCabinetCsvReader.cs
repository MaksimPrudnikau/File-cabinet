using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.Import
{
    public class FIleCabinetCsvReader
    {
        private readonly StreamReader _reader;
        
        public FIleCabinetCsvReader(StreamReader reader)
        {
            _reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();

            var readLines = _reader.ReadToEnd().Split(Environment.NewLine);

            for (int i = 1; i < readLines.Length; i++)
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

            IRecordValidator validator = new CustomValidator();

            return new FileCabinetRecord
            {
                Id = Parse(split[idIndex], InputConverter.IdConverter, validator.IdValidator),
                FirstName = Parse(split[firstNameIndex], InputConverter.NameConverter, validator.NameValidator),
                LastName = Parse(split[lastNameIndex], InputConverter.NameConverter, validator.NameValidator),
                DateOfBirth = Parse(split[dateOfBirthIndex], InputConverter.DateOfBirthConverter,
                    validator.DateOfBirthValidator),
                JobExperience = Parse(split[jobExperienceIndex], InputConverter.JobExperienceConverter,
                    validator.JobExperienceValidator),
                Wage = Parse(split[wageIndex], InputConverter.WageConverter, validator.WageValidator),
                Rank = Parse(split[rankIndex], InputConverter.RankConverter, validator.RankValidator)
            };
        }

        private static T Parse<T>(string source, Func<string, ConversionResult<T>> converter, Func<T, ValidationResult> validator)
        {
            var conversionResult = converter(source);

            if (!conversionResult.Parsed)
            {
                throw new ArgumentException($"{source} Error: wrong conversion");
            }

            var value = conversionResult.Result;

            var validationResult = validator(value);
            if (validationResult.Parsed)
            {
                return value;
            }
            
            throw new ArgumentException(validationResult.Message);
        }
    }
}