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
            switch (searchValue.Attribute)
            {
                case SearchValue.SearchAttribute.Id:
                    recordsToRemove = new List<FileCabinetRecord>
                        {Records[InputConverter.IdConverter(searchValue.Value).Result]};
                    break;
                
                case SearchValue.SearchAttribute.FirstName:
                    recordsToRemove = FirstNames[searchValue.Value];
                    break;
                
                case SearchValue.SearchAttribute.LastName:
                    recordsToRemove = LastNames[searchValue.Value];
                    break;
                
                case SearchValue.SearchAttribute.DateOfBirth:
                    recordsToRemove = DateOfBirths[InputConverter.DateOfBirthConverter(searchValue.Value).Result];
                    break;
                
                case SearchValue.SearchAttribute.JobExperience:
                    recordsToRemove = JobExperiences[InputConverter.JobExperienceConverter(searchValue.Value).Result];
                    break;
                
                case SearchValue.SearchAttribute.Salary:
                    recordsToRemove = Salaries[InputConverter.SalaryConverter(searchValue.Value).Result];
                    break;
                
                case SearchValue.SearchAttribute.Rank:
                    recordsToRemove = Ranks[InputConverter.RankConverter(searchValue.Value).Result];
                    break;
            }

            foreach (var record in recordsToRemove)
            {
                RemoveFromAllDictionaries(record);
                yield return record.Id;
            }
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