namespace FileCabinetApp
{
    public static class RecordValidatorConsts
    {
        // IdValidator
        public const string IdIsLessThenZero = "Id is less than zero";

        // NameValidator
        public const string NameIsNullOrWhiteSpace = "The name is null or whitespace";
        public const string NameWrongLength = "The name's length is less then 2 or greater than 100";
        public const string TheNameIsNotLettersOnly = "The name contains non-letter characters";
        
        // DateOfBirthValidator
        public const string DateOfBirthIsLessThanMinimal = "The date of birth is less than it`s minimal value";
        public const string DateOfBirthIsGreaterThanMaximal = "The date of birth is greater than it`s maximal value";
        
        // JobExperienceValidator
        public const string JobExperienceIsLessThanMinimal = "The job experience is less than it`s minimal value";
        public const string JobExperienceIsGreaterThanMaximal = "The job experience is greater than it`s minimal value";
        
        // WageValidator
        public const string WageIsLessThanMinimal = "Wage is less than it`s minimal value";
        
        // RankValidator
        public const string RankIsNotDefinedInGrades = "Rank is not defined in current grades";
    }
}