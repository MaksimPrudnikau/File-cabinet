using System;

namespace FileCabinetApp.FileCabinetService
{
    public class SearchValue
    {
        public enum SearchProperty
        {
            Id,
            FirstName,
            LastName,
            DateOfBirth,
            JobExperience,
            Salary,
            Rank
        }
        
        public SearchProperty Property { get; }

        public string Value { get; }

        public SearchValue(string attribute, string value)
        {
            Property = Enum.Parse<SearchProperty>(attribute, true);
            Value = value;
        }
    }
}