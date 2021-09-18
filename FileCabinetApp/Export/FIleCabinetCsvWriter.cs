using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    public class FIleCabinetCsvWriter
    {
        private readonly TextWriter _file;

        public FIleCabinetCsvWriter(TextWriter file)
        {
            _file = file;
            _file.WriteLine("Id,First Name,Last Name,Date of Birth,Job experience,Wage,Rank");
        }

        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                return;
            }

            _file.WriteLine($"{record.Id}," +
                            $"{record.FirstName}," +
                            $"{record.LastName}," +
                            $"{record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture).Replace('-', '/')}," +
                            $"{record.JobExperience}," +
                            $"{record.Wage}," +
                            $"{record.Rank}");
            
        }
    }
}