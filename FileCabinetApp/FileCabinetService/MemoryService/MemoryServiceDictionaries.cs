using System;
using System.Collections.Generic;
using System.Globalization;

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

        public IEnumerable<int> Remove(SearchValue attribute, string value)
        {
            var recordsToRemove = new List<FileCabinetRecord>();
            switch (attribute)
            {
                case SearchValue.FirstName:
                    recordsToRemove = FirstNames[value];
                    break;
                
                case SearchValue.LastName:
                    recordsToRemove = LastNames[value];
                    break;
                
                case SearchValue.DateOfBirth:
                    var dateOfBirth = DateTime.ParseExact(value, FileCabinetConsts.InputDateFormat,
                        CultureInfo.InvariantCulture);
                    recordsToRemove = DateOfBirths[dateOfBirth];
                    break;
                
                case SearchValue.JobExperience:
                    var jobExperience = Convert.ToInt16(value, CultureInfo.InvariantCulture);
                    recordsToRemove = JobExperiences[jobExperience];
                    break;
                
                case SearchValue.Salary:
                    var salary = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                    recordsToRemove = Salaries[salary];
                    break;
                
                case SearchValue.Rank:
                    var rank = Convert.ToChar(value, CultureInfo.InvariantCulture);
                    recordsToRemove = Ranks[rank];
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