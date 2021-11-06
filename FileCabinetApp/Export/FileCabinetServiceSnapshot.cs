using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Import;

namespace FileCabinetApp.Export
{
    public class FileCabinetServiceSnapshot
    {
        public IList<FileCabinetRecord> Records { get; private set; }

        public FileCabinetServiceSnapshot() { }
        
        public FileCabinetServiceSnapshot(IFileCabinetService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            
            Records = new List<FileCabinetRecord>(service.GetRecords());
        }

        /// <summary>
        /// Create csv file to source <see cref="StreamWriter"/> file
        /// </summary>
        /// <param name="file">Source file</param>
        public void SaveToCsv(StreamWriter file)
        {
            var csvWriter = new FIleCabinetCsvWriter(file);
            csvWriter.Write((FileCabinetRecord[]) Records);
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

            if (Records.Count <= 0) return;

            var classWriter = new FileCabinetXmlWriter(XmlWriter.Create(file, settings));

            classWriter.Write((FileCabinetRecord[]) Records);
        }

        public void LoadFromCsv(StreamReader reader)
        {
            var csvReader = new FIleCabinetCsvReader(reader);
            Records = csvReader.ReadAll();
        }

        public void LoadFromXml(StreamReader reader)
        {
            var xmlReader = new FileCabinetXmlReader(reader);
                Records = xmlReader.ReadAll();
            }

        /// <summary>
        /// Read all records from source file and delete it 
        /// </summary>
        /// <param name="file"></param>2
        /// <param name="service"></param>
        /// <returns><see cref="FileCabinetServiceSnapshot"/> snapshot created from source file</returns>
        /// <exception cref="ArgumentNullException">Source file or service are null</exception>
        public static FileCabinetServiceSnapshot CopyAndDelete(FileStream file, IFileCabinetService service)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            
            var snapshot = new FileCabinetServiceSnapshot(service);
            file.Close();
            File.Delete(file.Name);
            return snapshot;
        }
    }
}