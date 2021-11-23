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

        public static bool Equals(SearchValue val1, SearchValue val2)
        {
            if (val1 is null)
            {
                throw new ArgumentNullException(nameof(val1));
            }

            if (val2 is null)
            {
                throw new ArgumentNullException(nameof(val2));
            }

            return val1.Property == val2.Property &&
                   string.Equals(val1.Value, val2.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}