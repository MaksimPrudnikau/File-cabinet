using System.Collections;
using System.Collections.Generic;

namespace FileCabinetApp.FileCabinetService.Iterators
{
    public class MemoryIterator : IRecordIterator
    {
        private readonly IList<FileCabinetRecord> _records;
        private int _currentIndex;
        
        public MemoryIterator(IList<FileCabinetRecord> records)
        {
            _records = records;
            _currentIndex = 0;
        }
        
        public FileCabinetRecord GetNext()
        {
            return _records[_currentIndex++];
        }

        public bool HasMore()
        {
            return _currentIndex < _records.Count;
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            while (HasMore())
            {
                yield return GetNext();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}