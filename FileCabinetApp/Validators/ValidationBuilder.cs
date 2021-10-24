using System.Collections.Generic;
using FileCabinetApp.ValidationRules;

namespace FileCabinetApp
{
    public class ValidationBuilder
    {
        private List<IRecordValidator> _validators;

        private static ValidationRules.ValidationRules _rules;

        public ValidationBuilder()
        {
            _rules = ValidationRulesReader.ReadRules(FileCabinetConsts.ValidationRulesFileName);
        }

        public IRecordValidator CreateDefault()
        {
            _validators = new List<IRecordValidator>
            {
                new IdValidator(),
                new FirstNameValidator(_rules.Default.FirstName.MinValue, _rules.Default.FirstName.MaxValue),
                new LastNameValidator(_rules.Default.LastName.MinValue, _rules.Default.LastName.MaxValue),
                new DateOfBirthValidator(_rules.Default.DateOfBirth.MinValue, _rules.Default.DateOfBirth.MaxValue),
            };

            return new CompositeValidator(_validators);
        }

        public IRecordValidator CreateCustom()
        {
            _validators = new List<IRecordValidator>
            {
                new IdValidator(),
                new FirstNameValidator(_rules.Custom.FirstName.MinValue, _rules.Custom.FirstName.MaxValue),
                new LastNameValidator(_rules.Custom.LastName.MinValue, _rules.Custom.LastName.MaxValue),
                new DateOfBirthValidator(_rules.Custom.DateOfBirth.MinValue, _rules.Custom.DateOfBirth.MaxValue),
                
                new JobExperienceValidator(
                    _rules.Custom.JobExperience.MinValue, _rules.Custom.JobExperience.MaxValue),
                
                new SalaryValidator(_rules.Custom.Salary.MinValue, _rules.Custom.Salary.MaxValue),
                new RankValidator(_rules.Custom.Rank.Ranks)
            };

            return new CompositeValidator(_validators);
        }
    }
}