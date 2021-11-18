using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService.FileSystemService;

namespace FileCabinetApp.FileCabinetService.MemoryService
{
    public class MemoryServiceDictionaries
    {
        public SortedDictionary<int, FileCabinetRecord> Records { get; } = new();

        private Dictionary<SearchValue, List<FileCabinetRecord>> _memoization = new();


        /// <summary>
        /// Append record to all dictionaries
        /// </summary>
        /// <param name="record">Source record</param>
        public void Add(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            Records.Add(record.Id, record);
        }

        public IEnumerable<int> Remove(SearchValue searchValue)
        {
            if (searchValue is null)
            {
                throw new ArgumentNullException(nameof(searchValue));
            }

            var deleted = new List<FileCabinetRecord>(Find(searchValue));

            foreach (var record in deleted)
            {
                Remove(record);
                yield return record.Id;
            }
        }

        public IEnumerable<FileCabinetRecord> Find(SearchValue value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (_memoization.ContainsKey(value))
            {
                foreach (var record in _memoization[value])
                {
                    yield return record;
                }
            }
            else
            {
                _memoization.Add(value, new List<FileCabinetRecord>());
                foreach (var record in Records.Values)
                {
                    if (RecordHelper.Contains(record, value))
                    {
                        _memoization[value].Add(record);
                        yield return record;   
                    }
                }
            }
        }

        public void Edit(FileCabinetRecord from, FileCabinetRecord to)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            Remove(from);
            Add(to);
        }

        private void Remove(FileCabinetRecord record)
        {
            Records.Remove(record.Id);
        }
    }
}