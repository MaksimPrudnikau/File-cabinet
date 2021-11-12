using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Printers
{
    public interface IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}