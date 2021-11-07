using System;

namespace FileCabinetApp.FileCabinetService
{
    public class SearchValue
    {
        public enum SearchAttribute
        {
            Id,
            FirstName,
            LastName,
            DateOfBirth,
            JobExperience,
            Salary,
            Rank
        }
        
        public SearchAttribute Attribute { get; }

        public string Value { get; }

        public SearchValue(string attribute, string value)
        {
            Attribute = Enum.Parse<SearchAttribute>(attribute, true);
            Value = value;
        }
    }
}