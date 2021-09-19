using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly List<FileCabinetRecord> _records;

        public FileCabinetServiceSnapshot(IEnumerable<FileCabinetRecord> source)
        {
            _records = new List<FileCabinetRecord>(source);
        }

        /// <summary>
        /// Create csv file to source <see cref="StreamWriter"/> file
        /// </summary>
        /// <param name="file">Source file</param>
        public void SaveToCsv(StreamWriter file)
        {
            var csvWriter = new FIleCabinetCsvWriter(file);
            foreach (var item in _records)
            {
                csvWriter.Write(item);
            }
        }

        /// <summary>
        /// Create xml file to source <see cref="StreamWriter"/> file
        /// </summary>
        /// <param name="file">Source file</param>
        public void SaveToXml(StreamWriter file)
        {
            if (file is null)
            {
                return;
            }

            var settings = new XmlWriterSettings
            {
                Encoding           = Encoding.UTF8,
                Indent             = true,
                IndentChars        = "\t",
                NewLineHandling    = NewLineHandling.Replace
            };
            
            using var writer = XmlWriter.Create(file, settings);

            if (_records.Count > 0)
            {
                writer.WriteStartElement("records");

                var classWriter = new FileCabinetXmlWriter(writer);

                foreach (var item in _records)
                {
                    classWriter.Write(item);
                }

                writer.WriteEndElement();
            }
        }
    }
}