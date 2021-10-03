using System;

namespace FileCabinetApp
{
    public class Parameter
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short JobExperience { get; set; } = FileCabinetConsts.MinimalJobExperience;
        public decimal Wage { get; set; } = FileCabinetConsts.MinimalWage;
        public char Rank { get; set; } = FileCabinetConsts.MinimalRank;
    }
}