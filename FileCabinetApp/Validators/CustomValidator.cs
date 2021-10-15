using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
    {
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            var results = new List<ValidationResult>
            {
                CustomIdValidator.Validate(record.Id),
                CustomFirstNameValidator.Validate(record.FirstName),
                CustomLastNameValidator.Validate(record.LastName),
                CustomDateOfBirthValidator.Validate(record.DateOfBirth),
                CustomJobExperienceValidator.Validate(record.JobExperience),
                CustomSalaryValidator.Validate(record.Salary),
                CustomRankValidator.Validate(record.Rank)
            };

            foreach (var item in results)
            {
                if (!item.Parsed)
                {
                    throw new ArgumentException(item.Message);
                }
            }
        }
    }
}