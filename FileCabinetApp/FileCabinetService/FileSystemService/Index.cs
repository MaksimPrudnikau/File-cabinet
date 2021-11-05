using System;
using System.Collections.Generic;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public class Index
    {
        public Dictionary<int, HashSet<long>> Id { get; } = new();
        
        public Dictionary<string, HashSet<long>> FirstNames { get; } = new();
        
        public Dictionary<string, HashSet<long>> LastNames { get; } = new();
        
        public Dictionary<DateTime, HashSet<long>> DateOfBirths { get; } = new();

        public void Add(FileCabinetRecord record, long position)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            AddToDictionaries(record, position);
        }

        public void Edit(FileCabinetRecord from, FileCabinetRecord to, long position)
        {
            if (to is null)
            {
                throw new ArgumentNullException(nameof(to));
            }
            
            RemoveFromDictionaries(from);
            AddToDictionaries(to, position);
        }
        
        public void Clear()
        {
            Id.Clear();
            FirstNames.Clear();
            LastNames.Clear();
            DateOfBirths.Clear();
        }
        
        private void RemoveFromDictionaries(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            Id.Remove(record.Id);
            FirstNames.Remove(record.FirstName);
            LastNames.Remove(record.LastName);
            DateOfBirths.Remove(record.DateOfBirth);
        }

        private void AddToDictionaries(FileCabinetRecord record, long position)
        {
            UpdateDictionary(Id, record.Id, position);
            UpdateDictionary(FirstNames, record.FirstName, position);
            UpdateDictionary(LastNames, record.LastName, position);
            UpdateDictionary(DateOfBirths, record.DateOfBirth, position);
        }

        private static void UpdateDictionary<T>(IDictionary<T, HashSet<long>> dictionary, T key, long position)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(position);
            }
            else
            {
                dictionary.Add(key, new HashSet<long>());
                dictionary[key].Add(position);
            }
        }
    }
}