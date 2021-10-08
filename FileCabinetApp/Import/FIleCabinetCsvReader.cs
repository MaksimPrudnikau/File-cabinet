using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.Import
{
    public class FIleCabinetCsvReader
    {
        private StreamReader _reader;
        
        
        public FIleCabinetCsvReader(){}

        public FIleCabinetCsvReader(StreamReader reader)
        {
            _reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();

            var readLines = _reader.ReadToEnd().Split('\n');
            for (int i = 1; i < readLines.Length; i++)
            {
                records.Add(Deserialize(readLines[i]));
            }

            return records;
        }

        private FileCabinetRecord Deserialize(string csvLine)
        {
            throw new NotImplementedException();
        }
    }
}