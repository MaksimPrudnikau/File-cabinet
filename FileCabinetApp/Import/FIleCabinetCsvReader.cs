using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.Import
{
    public class FIleCabinetCsvReader
    {
        private readonly StreamReader _reader;
        
        public FIleCabinetCsvReader(StreamReader reader)
        {
            _reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();

            var readLines = _reader.ReadToEnd().Split(Environment.NewLine);

            for (int i = 1; i < readLines.Length; i++)
            {
                if (readLines[i].Length == 0) continue;
                
                try
                {
                    var record = FileCabinetRecord.Deserialize(readLines[i]);
                    records.Add(record);
                }
                catch (ArgumentException e)
                {
                    Console.Error.WriteLine($"Record at line {i} Error: {e.Message}");
                }
            }

            return records;
        }
    }
}