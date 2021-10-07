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
            throw new NotImplementedException();
        }
    }
}