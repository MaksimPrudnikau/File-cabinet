using System;

namespace FileCabinetApp
{
    public class FilesystemRecord
    {
        public short Status { get; }
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public short JobExperience { get; }
        public decimal Wage { get; }
        public char Rank { get; }

        public FilesystemRecord(Parameter parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            
            Status = 0;
            Id = parameter.Id;
            FirstName = parameter.FirstName;
            LastName = parameter.LastName;
            Year = parameter.DateOfBirth.Year;
            Month = parameter.DateOfBirth.Month;
            Day = parameter.DateOfBirth.Day;
            JobExperience = parameter.JobExperience;
            Wage = parameter.Wage;
            Rank = parameter.Rank;
        }
    }
}