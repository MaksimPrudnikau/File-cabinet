using System.Collections.Generic;

namespace FileCabinetApp
{
    public class ValidationBuilder
    {
        private List<IRecordValidator> _validators;

        public IRecordValidator CreateDefault()
        {
            _validators = new List<IRecordValidator>
            {
                new IdValidator(),
                new FirstNameValidator(FileCabinetConsts.MinimalNameLength, FileCabinetConsts.MaximalNameLength),
                new LastNameValidator(FileCabinetConsts.MinimalNameLength, FileCabinetConsts.MaximalNameLength),
                new DateOfBirthValidator(FileCabinetConsts.MinimalDateTime, FileCabinetConsts.MaximalDateTime),
            };

            return new CompositeValidator(_validators);
        }

        public IRecordValidator CreateCustom()
        {
            _validators = new List<IRecordValidator>
            {
                new IdValidator(),
                new FirstNameValidator(FileCabinetConsts.MinimalNameLength, FileCabinetConsts.MaximalNameLength),
                new LastNameValidator(FileCabinetConsts.MinimalNameLength, FileCabinetConsts.MaximalNameLength),
                new DateOfBirthValidator(FileCabinetConsts.MinimalDateTime, FileCabinetConsts.MaximalDateTime),
                
                new JobExperienceValidator(
                    FileCabinetConsts.MinimalJobExperience, FileCabinetConsts.MaximalJobExperience),
                
                new SalaryValidator(FileCabinetConsts.MinimalSalary),
                new RankValidator()
            };

            return new CompositeValidator(_validators);
        }
    }
}