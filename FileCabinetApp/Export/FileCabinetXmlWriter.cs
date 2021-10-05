using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    public class FileCabinetXmlWriter
    {
        private readonly XmlWriter _writer;

        public FileCabinetXmlWriter(XmlWriter file)
        {
            _writer = file;
        }

        /// <summary>
        /// Serialize record to xml format
        /// </summary>
        /// <param name="records">Source record</param>
        public void Write(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            var serializer = new XmlSerializer(typeof(SerializableRecord));
            
            serializer.Serialize(_writer, ToSerializableRecord(records));
        }

        private static SerializableRecord ToSerializableRecord(IEnumerable<FileCabinetRecord> records)
        {
            var array = new Collection<RecordXml>();
            foreach (var item in records)
            {
                array.Add(new RecordXml
                {
                    Id = item.Id,
                    Name = new NameXml{First = item.FirstName, Last = item.LastName},
                    DateOfBirth = item.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    JobExperience = item.JobExperience,
                    Wage = item.Wage,
                    Rank = item.Rank
                });
            }

            return new SerializableRecord(array);
        }
    }
}