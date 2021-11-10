using System;
using System.Collections.Generic;

namespace FileCabinetApp.FileCabinetService.MemoryService
{
    public class MemoryServiceDictionaries
    {
        public SortedDictionary<int, FileCabinetRecord> Records { get; } = new();

        public Dictionary<string, List<FileCabinetRecord>> FirstNames { get; } = new();

        public Dictionary<string, List<FileCabinetRecord>> LastNames { get; } = new();

        public Dictionary<DateTime, List<FileCabinetRecord>> DateOfBirths { get; } = new();

        public Dictionary<short, List<FileCabinetRecord>> JobExperiences { get; } = new();

        public Dictionary<decimal, List<FileCabinetRecord>> Salaries { get; } = new();

        public Dictionary<char, List<FileCabinetRecord>> Ranks { get; } = new();


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
            
            AppendToAllDictionaries(record);
        }

        public void Remove(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            RemoveFromAllDictionaries(record);
        }

        public IEnumerable<int> Remove(SearchValue searchValue)
        {
            if (searchValue is null)
            {
                throw new ArgumentNullException(nameof(searchValue));
            }
            
            var recordsToRemove = new List<FileCabinetRecord>();
            switch (searchValue.Property)
            {
                case SearchValue.SearchProperty.Id:
                    recordsToRemove = new List<FileCabinetRecord>
                        {Records[InputConverter.IdConverter(searchValue.Value).Result]};
                    break;
                
                case SearchValue.SearchProperty.FirstName:
                    recordsToRemove = FirstNames[searchValue.Value];
                    break;
                
                case SearchValue.SearchProperty.LastName:
                    recordsToRemove = LastNames[searchValue.Value];
                    break;
                
                case SearchValue.SearchProperty.DateOfBirth:
                    recordsToRemove = DateOfBirths[InputConverter.DateOfBirthConverter(searchValue.Value).Result];
                    break;
                
                case SearchValue.SearchProperty.JobExperience:
                    recordsToRemove = JobExperiences[InputConverter.JobExperienceConverter(searchValue.Value).Result];
                    break;
                
                case SearchValue.SearchProperty.Salary:
                    recordsToRemove = Salaries[InputConverter.SalaryConverter(searchValue.Value).Result];
                    break;
                
                case SearchValue.SearchProperty.Rank:
                    recordsToRemove = Ranks[InputConverter.RankConverter(searchValue.Value).Result];
                    break;
            }

            foreach (var record in recordsToRemove)
            {
                RemoveFromAllDictionaries(record);
                yield return record.Id;
            }
        }

        public IEnumerable<FileCabinetRecord> Find(SearchValue value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            if (value.Property is SearchValue.SearchProperty.Id)
            {
                return new List<FileCabinetRecord>{Records[InputConverter.IdConverter(value.Value).Result]};
            }

            return value.Property switch
            {
                SearchValue.SearchProperty.FirstName => FirstNames[InputConverter.NameConverter(value.Value).Result],
                SearchValue.SearchProperty.LastName => LastNames[InputConverter.NameConverter(value.Value).Result],
                SearchValue.SearchProperty.DateOfBirth => DateOfBirths[
                    InputConverter.DateOfBirthConverter(value.Value).Result],
                SearchValue.SearchProperty.JobExperience => JobExperiences[
                    InputConverter.JobExperienceConverter(value.Value).Result],
                SearchValue.SearchProperty.Salary => Salaries[InputConverter.SalaryConverter(value.Value).Result],
                SearchValue.SearchProperty.Rank => Ranks[InputConverter.RankConverter(value.Value).Result],
                
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        public void Edit(FileCabinetRecord from, FileCabinetRecord to)
        {
            Remove(from);
            Add(to);
        }

        private void AppendToAllDictionaries(FileCabinetRecord record)
        {
            Records.Add(record.Id, record);
            AppendToDictionary(FirstNames, record.FirstName, record);
            AppendToDictionary(LastNames, record.LastName, record);
            AppendToDictionary(DateOfBirths, record.DateOfBirth, record);
            AppendToDictionary(JobExperiences, record.JobExperience, record);
            AppendToDictionary(Salaries, record.Salary, record);
            AppendToDictionary(Ranks, record.Rank, record);
        }
        
        private static void AppendToDictionary<T>(IDictionary<T, List<FileCabinetRecord>> dictionary, T key, 
            FileCabinetRecord value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, new List<FileCabinetRecord> { value });
            }
            else
            {
                dictionary[key].Add(value);
            }
        }

        /// <summary>
        /// Remove record from all dictionaries
        /// </summary>
        /// <param name="record">Source record to remove</param>
        private void RemoveFromAllDictionaries(FileCabinetRecord record)
        {
            Records.Remove(record.Id);
            FirstNames.Remove(record.FirstName);
            LastNames.Remove(record.LastName);
            DateOfBirths.Remove(record.DateOfBirth);
            Salaries.Remove(record.Salary);
            Ranks.Remove(record.Rank);
        }
    }
}