using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Import
{
    public class FileCabinetXmlReader
    {
        private readonly StreamReader _reader;
        public FileCabinetXmlReader(StreamReader reader)
        {
            _reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            var serializer = new XmlSerializer(typeof(RecordsXml));

            using var xmlReader = XmlReader.Create(_reader);
            
            var records = (RecordsXml)serializer.Deserialize(xmlReader) ?? new RecordsXml();

            var fileCabinetRecords = new List<FileCabinetRecord>();
            
            foreach (var item in records.Records)
            {
                fileCabinetRecords.Add(ToFileCabinetRecord(item));
            }
            
            return fileCabinetRecords;
        }

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