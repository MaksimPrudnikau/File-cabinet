using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService.FileSystemService;

namespace FileCabinetApp.FileCabinetService.MemoryService
{
    public class MemoryServiceDictionaries
    {
        public SortedDictionary<int, FileCabinetRecord> Records { get; } = new();

        private readonly Dictionary<SearchValue, List<FileCabinetRecord>> _memoization = new();


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

        /// <summary>
        /// Remove all records satisfy source value
        /// </summary>
        /// <param name="searchValue">Source value to search</param>
        /// <returns>Identifications of deleted records</returns>
        /// <exception cref="ArgumentNullException">The search value is null</exception>
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

        /// <summary>
        /// Find all records satisfy the source value
        /// </summary>
        /// <param name="value">Source value to search</param>
        /// <returns>An array of records satisfy the source value</returns>
        /// <exception cref="ArgumentNullException">The search value is null</exception>
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

        /// <summary>
        /// Edit the existing record with the new one
        /// </summary>
        /// <param name="from">The already existing record</param>
        /// <param name="to">The new record</param>
        /// <exception cref="ArgumentNullException">At least one of the source values is null</exception>
        public void Edit(FileCabinetRecord from, FileCabinetRecord to)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            Remove(from);
            Add(to);
        }

        /// <summary>
        /// Remove source record from dictionary
        /// </summary>
        /// <param name="record">Source record</param>
        private void Remove(FileCabinetRecord record)
        {
            Records.Remove(record.Id);
        }
    }
}