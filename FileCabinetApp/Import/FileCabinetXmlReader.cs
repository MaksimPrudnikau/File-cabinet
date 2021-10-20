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
        private static readonly IRecordValidator Validator = new ValidationBuilder().CreateCustom();
        
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
                    Validator.Validate(ToFileCabinetRecord(item));
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
                Salary = record.Wage,
                Rank = record.Rank
            };
        }
    }
}