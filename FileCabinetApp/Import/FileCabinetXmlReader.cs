using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.Import
{
    public class FileCabinetXmlReader
    {
        private readonly StreamReader _reader;
        public FileCabinetXmlReader(StreamReader reader)
        {
            _reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            throw new NotImplementedException();
        }
    }
}