using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using FileCabinetApp.Import;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly List<FileCabinetRecord> _records;

        public IReadOnlyCollection<FileCabinetRecord> Records { get; set; }

        public FileCabinetServiceSnapshot(){}

        public FileCabinetServiceSnapshot(IEnumerable<FileCabinetRecord> source)
        {
            _records = new List<FileCabinetRecord>(source);
            Records = new List<FileCabinetRecord>();
        }

        /// <summary>
        /// Create csv file to source <see cref="StreamWriter"/> file
        /// </summary>
        /// <param name="file">Source file</param>
        public void SaveToCsv(StreamWriter file)
        {
            var csvWriter = new FIleCabinetCsvWriter(file);
            csvWriter.Write(_records.ToArray());
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
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "\t",
                NewLineHandling = NewLineHandling.Replace
            };

            if (_records.Count <= 0) return;

            var classWriter = new FileCabinetXmlWriter(XmlWriter.Create(file, settings));

            classWriter.Write(_records.ToArray());
        }

        public void LoadFromCsv(StreamReader reader)
        {
            var csvReader = new FIleCabinetCsvReader(reader);
            Records = (IReadOnlyCollection<FileCabinetRecord>) csvReader.ReadAll();
        }
    }
}