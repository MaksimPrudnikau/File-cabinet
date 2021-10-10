using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace FileCabinetApp.Import
{
    public class FIleCabinetCsvReader
    {
        private StreamReader _reader;
        
        public FIleCabinetCsvReader(StreamReader reader)
        {
            _reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();

            var readLines = _reader.ReadToEnd().Split(Environment.NewLine);

            var record = new FileCabinetRecord();
            
            for (int i = 1; i < readLines.Length; i++)
            {
                if (readLines[i].Length == 0) continue;
                
                try
                {
                    record = FileCabinetRecord.Deserialize(readLines[i]);
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