
using System.Collections.Generic;

namespace FileCabinetApp.FileCabinetService.Iterators
{
    public interface IRecordIterator : IEnumerable<FileCabinetRecord>
    {
        public FileCabinetRecord GetNext();

        public bool HasMore();
    }
}