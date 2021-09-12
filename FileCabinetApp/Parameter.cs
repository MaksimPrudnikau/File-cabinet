using System;

namespace FileCabinetApp
{
    public class Parameter
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short JobExperience { get; set; }
        public decimal Wage { get; set; }
        public char Rank { get; set; }
    }
}