using System;
using System.Collections.Generic;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public class FileSystemServiceDictionaries
    {
        public Dictionary<int, HashSet<long>> Id { get; } = new();
        
        public Dictionary<string, HashSet<long>> FirstNames { get; } = new();
        
        public Dictionary<string, HashSet<long>> LastNames { get; } = new();
        
        public Dictionary<DateTime, HashSet<long>> DateOfBirths { get; } = new();
        
        public Dictionary<short, HashSet<long>> JobExperiences { get; } = new();
        
        public Dictionary<decimal, HashSet<long>> Salaries { get; } = new();
        
        public Dictionary<char, HashSet<long>> Ranks { get; } = new();

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

        public HashSet<long> GetPositionsByValue(SearchValue searchValue)
        {
            if (searchValue is null)
            {
                throw new ArgumentNullException(nameof(searchValue));
            }

            return searchValue.Attribute switch
            {
                SearchValue.SearchAttribute.Id => Id[InputConverter.IdConverter(searchValue.Value).Result],
                
                SearchValue.SearchAttribute.FirstName => FirstNames[InputConverter.NameConverter(searchValue.Value).Result],
                
                SearchValue.SearchAttribute.LastName => LastNames[InputConverter.NameConverter(searchValue.Value).Result],
                
                SearchValue.SearchAttribute.DateOfBirth => DateOfBirths[InputConverter.DateOfBirthConverter(searchValue.Value).Result],
                
                SearchValue.SearchAttribute.JobExperience => JobExperiences[InputConverter.JobExperienceConverter(searchValue.Value).Result],
                
                SearchValue.SearchAttribute.Salary => Salaries[InputConverter.SalaryConverter(searchValue.Value).Result],
                
                SearchValue.SearchAttribute.Rank => Ranks[InputConverter.RankConverter(searchValue.Value).Result],
                
                _ => throw new ArgumentOutOfRangeException(nameof(searchValue))
            };
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
            JobExperiences.Remove(record.JobExperience);
            Salaries.Remove(record.Salary);
            Ranks.Remove(record.Rank);
        }

        private void AddToDictionaries(FileCabinetRecord record, long position)
        {
            UpdateDictionary(Id, record.Id, position);
            UpdateDictionary(FirstNames, record.FirstName, position);
            UpdateDictionary(LastNames, record.LastName, position);
            UpdateDictionary(DateOfBirths, record.DateOfBirth, position);
            UpdateDictionary(JobExperiences, record.JobExperience, position);
            UpdateDictionary(Salaries, record.Salary, position);
            UpdateDictionary(Ranks, record.Rank, position);
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