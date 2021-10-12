using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Import
{
    public class FileCabinetXmlReader
    {
        private readonly StreamReader _reader;
        private static readonly XmlSerializer Serializer = new (typeof(RecordsXml));
        
        public FileCabinetXmlReader(StreamReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Read all records from current base file written in xml
        /// </summary>
        /// <returns><see cref="IList{T}"/> of <see cref="FileCabinetRecord"/></returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            using var xmlReader = XmlReader.Create(_reader);

            var records = (RecordsXml) Serializer.Deserialize(xmlReader) ?? new RecordsXml();

            var fileCabinetRecords = new List<FileCabinetRecord>();
            
            foreach (var item in records.Records)
            {
                try
                {
                    Validate(item);
                }
                catch (Exception exception) when (exception is ArgumentException or FormatException)
                {
                    Console.Error.WriteLine($"Record #{item.Id}: {exception.Message}");
                    continue;
                }
                
                fileCabinetRecords.Add(ToFileCabinetRecord(item));
            }
            
            return fileCabinetRecords;
        }

        /// <summary>
        /// Validate source record with rules of base validator
        /// </summary>
        /// <param name="record">Source record</param>
        /// <exception cref="ArgumentNullException">record is null</exception>
        private static void Validate(RecordXml record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var validator = new CustomValidator();
            
            ThrowIfWrong(validator.IdValidator(record.Id));

            ThrowIfWrong(validator.NameValidator(record.Name.First));
            ThrowIfWrong(validator.NameValidator(record.Name.Last));
            var dateOfBirth = DateTime.ParseExact(record.DateOfBirth, FileCabinetConsts.InputDateFormat,
                CultureInfo.InvariantCulture);
            ThrowIfWrong(validator.DateOfBirthValidator(dateOfBirth));
            ThrowIfWrong(validator.JobExperienceValidator(record.JobExperience));
            ThrowIfWrong(validator.WageValidator(record.Wage));
            ThrowIfWrong(validator.RankValidator(record.Rank));
        }

        private static void ThrowIfWrong(ValidationResult result)
        {
            if (!result.Parsed)
            {
                throw new ArgumentException(result.Message);
            }
        }

        /// <summary>
        /// Parse <see cref="RecordXml"/> record to <see cref="FileCabinetRecord"/>
        /// </summary>
        /// <param name="record">Source record</param>
        /// <returns>Suitable <see cref="FileCabinetRecord"/> object</returns>
        private static FileCabinetRecord ToFileCabinetRecord(RecordXml record)
        {
            return new FileCabinetRecord
            {
                Id = record.Id,
                FirstName = record.Name.First,
                LastName = record.Name.Last,
                DateOfBirth = DateTime.ParseExact(record.DateOfBirth, FileCabinetConsts.InputDateFormat, CultureInfo.InvariantCulture),
                JobExperience = record.JobExperience,
                Wage = record.Wage,
                Rank = record.Rank
            };
        }
    }
}