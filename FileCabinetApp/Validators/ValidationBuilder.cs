using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Validation;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    public class ValidationBuilder
    {
        private List<IRecordValidator> _validators;

        private static Validation.ValidationRules _validationRules;

        public ValidationBuilder()
        {
            _validationRules = ValidationRulesReader.ReadRules(FileCabinetConsts.ValidationRulesFileName);
        }

        public IRecordValidator CreateDefault()
        {
            _validators = new List<IRecordValidator>
            {
                new IdValidator(),
                new FirstNameValidator(_validationRules.Default.FirstName.MinValue, _validationRules.Default.FirstName.MaxValue),
                new LastNameValidator(_validationRules.Default.LastName.MinValue, _validationRules.Default.LastName.MaxValue),
                new DateOfBirthValidator(_validationRules.Default.DateOfBirth.MinValue, _validationRules.Default.DateOfBirth.MaxValue),
            };

            return new CompositeValidator(_validators);
        }

        public IRecordValidator CreateCustom()
        {
            _validators = new List<IRecordValidator>
            {
                new IdValidator(),
                new FirstNameValidator(_validationRules.Custom.FirstName.MinValue, _validationRules.Custom.FirstName.MaxValue),
                new LastNameValidator(_validationRules.Custom.LastName.MinValue, _validationRules.Custom.LastName.MaxValue),
                new DateOfBirthValidator(_validationRules.Custom.DateOfBirth.MinValue, _validationRules.Custom.DateOfBirth.MaxValue),
                
                new JobExperienceValidator(
                    _validationRules.Custom.JobExperience.MinValue, _validationRules.Custom.JobExperience.MaxValue),
                
                new SalaryValidator(_validationRules.Custom.Salary.MinValue, _validationRules.Custom.Salary.MaxValue),
                new RankValidator(_validationRules.Custom.Rank.Ranks)
            };

            return new CompositeValidator(_validators);
        }
    }
}