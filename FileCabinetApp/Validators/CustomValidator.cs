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

            new IdValidator()
                .Validate(record);
            new FirstNameValidator(FileCabinetConsts.MinimalNameLength, FileCabinetConsts.MaximalNameLength)
                .Validate(record.FirstName);
            new LastNameValidator(FileCabinetConsts.MinimalNameLength, FileCabinetConsts.MaximalNameLength)
                .Validate(record.LastName);
            new DateOfBirthValidator(FileCabinetConsts.MinimalDateTime, FileCabinetConsts.MaximalDateTime)
                .Validate(record.DateOfBirth);
            new JobExperienceValidator(FileCabinetConsts.MinimalJobExperience, FileCabinetConsts.MaximalJobExperience)
                .Validate(record.JobExperience);
            new SalaryValidator(FileCabinetConsts.MinimalSalary)
                .Validate(record.Salary);
            new RankValidator()
                .Validate(record);
        }
    }
}