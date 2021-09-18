using System;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        
        public short JobExperience { get; set; }
        
        public decimal Wage { get; set; }
        
        public char Rank { get; set; }

        public void Print()
        {
            Console.WriteLine(EnglishSource.print_record,
                Id,
                FirstName,
                LastName,
                DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                JobExperience,
                Wage,
                Rank);
        }
    }
}