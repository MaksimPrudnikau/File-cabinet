using System;
using System.Globalization;
using System.Xml;

namespace FileCabinetApp
{
    public class FileCabinetXmlWriter
    {
        private readonly XmlWriter _writer;

        public FileCabinetXmlWriter(XmlWriter file)
        {
            _writer = file;
        }

        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            _writer.WriteStartElement("record");
            _writer.WriteStartAttribute("id");
            _writer.WriteValue(record.Id);
            _writer.WriteEndAttribute();

            _writer.WriteStartElement("name");
            _writer.WriteStartAttribute("first");
            _writer.WriteValue(record.FirstName);
            _writer.WriteEndAttribute();

            _writer.WriteStartAttribute("last");
            _writer.WriteValue(record.LastName);
            _writer.WriteEndAttribute();
            _writer.WriteEndElement();

            _writer.WriteStartElement("dateOfBirth");
            _writer.WriteValue(record.DateOfBirth.ToString("d", CultureInfo.InvariantCulture));
            _writer.WriteEndElement();

            _writer.WriteStartElement("jobExperience");
            _writer.WriteValue(record.JobExperience);
            _writer.WriteEndElement();

            _writer.WriteStartElement("wage");
            _writer.WriteValue(record.Wage);
            _writer.WriteEndElement();

            _writer.WriteStartElement("rank");
            _writer.WriteValue(record.Rank);
            _writer.WriteEndElement();

            _writer.WriteEndElement();
        }
    }
}