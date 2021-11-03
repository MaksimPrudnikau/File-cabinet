using System;
using System.Collections.Generic;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public class Index
    {
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

        private void RemoveFromDictionaries(FileCabinetRecord record)
        {
            RemoveFromFirstNames(record);
            RemoveFromLastNames(record);
            RemoveFromDateOfBirths(record);
        }

        private void RemoveFromFirstNames(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            FirstNames.Remove(record.FirstName);
        }
        
        private void RemoveFromLastNames(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            LastNames.Remove(record.LastName);
        }
        
        private void RemoveFromDateOfBirths(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            DateOfBirths.Remove(record.DateOfBirth);
        }

        private void AddToDictionaries(FileCabinetRecord record, long position)
        {
            UpdateFirstNames(record.FirstName, position);
            UpdateLastNames(record.LastName, position);
            UpdateDateOfBirths(record.DateOfBirth, position);
        }

        private void UpdateFirstNames(string firstname, long position)
        {
            if (FirstNames.ContainsKey(firstname))
            {
                FirstNames[firstname].Add(position);
            }
            else
            {
                FirstNames.Add(firstname, new HashSet<long>());
                FirstNames[firstname].Add(position);
            }
        }
        
        private void UpdateLastNames(string lastName, long position)
        {
            if (LastNames.ContainsKey(lastName))
            {
                LastNames[lastName].Add(position);
            }
            else
            {
                LastNames.Add(lastName, new HashSet<long>());
                LastNames[lastName].Add(position);
            }
        }
        
        private void UpdateDateOfBirths(DateTime dateOfBirth, long position)
        {
            if (DateOfBirths.ContainsKey(dateOfBirth))
            {
                DateOfBirths[dateOfBirth].Add(position);
            }
            else
            {
                DateOfBirths.Add(dateOfBirth, new HashSet<long>());
                DateOfBirths[dateOfBirth].Add(position);
            }
        }
    }
}