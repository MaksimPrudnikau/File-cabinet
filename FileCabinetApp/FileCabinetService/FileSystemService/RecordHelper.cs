using System;
namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public static class RecordHelper
    {
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
    }
}