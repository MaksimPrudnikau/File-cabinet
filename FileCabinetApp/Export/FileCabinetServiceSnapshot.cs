using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly List<FileCabinetRecord> _records;

        public FileCabinetServiceSnapshot(IEnumerable<FileCabinetRecord> source)
        {
            _records = new List<FileCabinetRecord>(source);
        }

        public void SaveToCsv(StreamWriter file)
        {
            var csvWriter = new FIleCabinetCsvWriter(file);
            foreach (var item in _records)
            {
                csvWriter.Write(item);
            }
        }
    }
}