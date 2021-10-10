using System;

namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        public ValidationResult IdValidator(int id);
        
        public ValidationResult NameValidator(string name);

        public ValidationResult DateOfBirthValidator(DateTime dateOfBirth);

        public ValidationResult JobExperienceValidator(short jobExperience);
        
        public ValidationResult WageValidator(decimal wage);
        
        public ValidationResult RankValidator(char rank);
    }
}