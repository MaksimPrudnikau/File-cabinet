using System;
using System.Globalization;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public static class RecordHelper
    {
        /// <summary>
        /// Edit the record with source <see cref="SearchValue"/>
        /// </summary>
        /// <param name="record">The record to edit</param>
        /// <param name="attribute">The object contains the attribute to edit and it's value</param>
        /// <returns>Edited record</returns>
        /// <exception cref="ArgumentNullException">At least one of the source values is null</exception>
        public static FileCabinetRecord EditByAttribute(FileCabinetRecord record, SearchValue attribute)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (attribute is null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }
            
            switch (attribute.Property)
            {
                case SearchValue.SearchProperty.Id:
                    record.Id = InputConverter.IdConverter(attribute.Value).Result;
                    break;
                case SearchValue.SearchProperty.FirstName:
                    record.FirstName = attribute.Value;
                    break;
                case SearchValue.SearchProperty.LastName:
                    record.LastName = attribute.Value;
                    break;
                case SearchValue.SearchProperty.DateOfBirth:
                    record.DateOfBirth = InputConverter.DateOfBirthConverter(attribute.Value).Result;
                    break;
                case SearchValue.SearchProperty.JobExperience:
                    record.JobExperience = InputConverter.JobExperienceConverter(attribute.Value).Result;
                    break;
                case SearchValue.SearchProperty.Salary:
                    record.Salary = InputConverter.SalaryConverter(attribute.Value).Result;
                    break;
                case SearchValue.SearchProperty.Rank:
                    record.Rank = InputConverter.RankConverter(attribute.Value).Result;
                    break;
            }

            return record;
        }

        /// <summary>
        /// Determine whether the source record has the attribute equals to the search value
        /// </summary>
        /// <param name="record">The source record</param>
        /// <param name="value">The object contains the attribute and it's value</param>
        /// <returns>True if contains</returns>
        /// <exception cref="ArgumentNullException">At least one of the source values is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">The attribute is out of the existing ones</exception>
        public static bool Contains(FileCabinetRecord record, SearchValue value)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            return value.Property switch
            {
                SearchValue.SearchProperty.Id => 
                    record.Id == InputConverter.IdConverter(value.Value).Result,
                
                SearchValue.SearchProperty.FirstName => 
                    record.FirstName == value.Value,
                
                SearchValue.SearchProperty.LastName => 
                    record.LastName == value.Value,
                
                SearchValue.SearchProperty.DateOfBirth => 
                    record.DateOfBirth == InputConverter.DateOfBirthConverter(value.Value).Result,
                
                SearchValue.SearchProperty.JobExperience => 
                    record.JobExperience == InputConverter.JobExperienceConverter(value.Value).Result,
                
                SearchValue.SearchProperty.Salary => 
                    record.Salary == InputConverter.SalaryConverter(value.Value).Result,
                
                SearchValue.SearchProperty.Rank => 
                    record.Rank == InputConverter.RankConverter(value.Value).Result,
                
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        /// <summary>
        /// Create new record equals to the source one
        /// </summary>
        /// <param name="source">The source record</param>
        /// <returns>The new <see cref="FileCabinetRecord"/></returns>
        /// <exception cref="ArgumentNullException">The source record is null</exception>
        public static FileCabinetRecord Clone(FileCabinetRecord source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            return new FileCabinetRecord
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth,
                JobExperience = source.JobExperience,
                Salary = source.Salary,
                Rank = source.Rank
            };
        }

        /// <summary>
        /// Extract the suitable attribute from the source record according to attribute
        /// </summary>
        /// <param name="record">The source record</param>
        /// <param name="attribute">Attribute to extract</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">At least one of the source values is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">The attribute is out of the existing ones</exception>
        public static string GetByAttribute(FileCabinetRecord record, SearchValue.SearchProperty attribute)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            return attribute switch
            {
                SearchValue.SearchProperty.Id => $"{record.Id}",
                SearchValue.SearchProperty.FirstName => record.FirstName,
                SearchValue.SearchProperty.LastName => record.LastName,
                SearchValue.SearchProperty.DateOfBirth => record.DateOfBirth.ToString(
                    FileCabinetConsts.OutputDateFormat, CultureInfo.InvariantCulture),
                SearchValue.SearchProperty.JobExperience => $"{record.JobExperience}",
                SearchValue.SearchProperty.Salary => $"{record.Salary}",
                SearchValue.SearchProperty.Rank => $"{record.Rank}",
                _ => throw new ArgumentOutOfRangeException(nameof(attribute))
            };
        }
    }
}