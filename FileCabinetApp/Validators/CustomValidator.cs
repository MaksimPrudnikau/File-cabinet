using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class CustomValidator : CompositeValidator
    {
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
        }

        public CustomValidator() : 
            base(new IRecordValidator[]
            {
                new IdValidator(),
                new FirstNameValidator(FileCabinetConsts.MinimalNameLength,FileCabinetConsts.MaximalNameLength),
                new LastNameValidator(FileCabinetConsts.MinimalNameLength,FileCabinetConsts.MaximalNameLength),
                new DateOfBirthValidator(FileCabinetConsts.MinimalDateTime, FileCabinetConsts.MaximalDateTime),
                new JobExperienceValidator(FileCabinetConsts.MinimalJobExperience,FileCabinetConsts.MaximalJobExperience),
                new SalaryValidator(FileCabinetConsts.MinimalSalary),
                new RankValidator()
            }){}
    }
}