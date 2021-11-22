using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Import
{
    public class FileCabinetXmlReader
    {
        private readonly StreamReader _reader;
        private readonly XmlSerializer _serializer;
        private readonly IRecordValidator _validator;
        
        public FileCabinetXmlReader(StreamReader reader)
        {
            _reader = reader;
            _serializer = new XmlSerializer(typeof(RecordsXml));
            _validator = new ValidationBuilder().CreateCustom();
        }

        /// <summary>
        /// Read all records from current base file written in xml
        /// </summary>
        /// <returns><see cref="IList{T}"/> of <see cref="FileCabinetRecord"/></returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            using var xmlReader = XmlReader.Create(_reader);
            var records = (RecordsXml) _serializer.Deserialize(xmlReader) ?? new RecordsXml();
            var fileCabinetRecords = new List<FileCabinetRecord>();
            
            foreach (var item in records.Records)
            {
                try
                {
                    _validator.Validate(ToFileCabinetRecord(item));
                }
                catch (Exception exception) when (exception is ArgumentException or FormatException)
                {
                    Console.Error.WriteLine(EnglishSource.Record_number_error, item.Id, exception.Message);
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