using System.Collections.Generic;

namespace FileCabinetApp.Handlers
{
    public interface IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}