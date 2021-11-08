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
            
            switch (attribute.Attribute)
            {
                case SearchValue.SearchAttribute.Id:
                    record.Id = InputConverter.IdConverter(attribute.Value).Result;
                    break;
                case SearchValue.SearchAttribute.FirstName:
                    record.FirstName = attribute.Value;
                    break;
                case SearchValue.SearchAttribute.LastName:
                    record.LastName = attribute.Value;
                    break;
                case SearchValue.SearchAttribute.DateOfBirth:
                    record.DateOfBirth = InputConverter.DateOfBirthConverter(attribute.Value).Result;
                    break;
                case SearchValue.SearchAttribute.JobExperience:
                    record.JobExperience = InputConverter.JobExperienceConverter(attribute.Value).Result;
                    break;
                case SearchValue.SearchAttribute.Salary:
                    record.Salary = InputConverter.SalaryConverter(attribute.Value).Result;
                    break;
                case SearchValue.SearchAttribute.Rank:
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
            
            return value.Attribute switch
            {
                SearchValue.SearchAttribute.Id => 
                    record.Id == InputConverter.IdConverter(value.Value).Result,
                
                SearchValue.SearchAttribute.FirstName => 
                    record.FirstName == value.Value,
                
                SearchValue.SearchAttribute.LastName => 
                    record.LastName == value.Value,
                
                SearchValue.SearchAttribute.DateOfBirth => 
                    record.DateOfBirth == InputConverter.DateOfBirthConverter(value.Value).Result,
                
                SearchValue.SearchAttribute.JobExperience => 
                    record.JobExperience == InputConverter.JobExperienceConverter(value.Value).Result,
                
                SearchValue.SearchAttribute.Salary => 
                    record.Salary == InputConverter.SalaryConverter(value.Value).Result,
                
                SearchValue.SearchAttribute.Rank => 
                    record.Rank == InputConverter.RankConverter(value.Value).Result,
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