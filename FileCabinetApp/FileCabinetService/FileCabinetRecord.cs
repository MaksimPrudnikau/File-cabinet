using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short JobExperience { get; set; } = FileCabinetConsts.MinimalJobExperience;

        public decimal Salary { get; set; } = FileCabinetConsts.MinimalSalary;

        public char Rank { get; set; } = FileCabinetConsts.Grades[0];
    }
}