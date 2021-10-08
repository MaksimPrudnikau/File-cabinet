using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    public class FIleCabinetCsvWriter
    {
        private TextWriter _file;

        /// <summary>
        /// Create <see cref="FIleCabinetCsvWriter"/> object by source <see cref="TextWriter"/> stream
        /// </summary>
        /// <param name="file">Source stream</param>
        public FIleCabinetCsvWriter(TextWriter file)
        {
            _file = file;
            _file.WriteLine("Id,First Name,Last Name,Date of Birth,Job experience,Wage,Rank");
        }

        /// <summary>
        /// Serialize source record to current <see cref="TextWriter"/>
        /// </summary>
        /// <param name="record"></param>
        public void Write(FileCabinetRecord[] record)
        {
            if (record is null)
            {
                return;
            }

            foreach (var item in record)
            {
                _file.WriteLine($"{item.Id}," +
                                $"{item.FirstName}," +
                                $"{item.LastName}," +
                                $"{item.DateOfBirth.ToString("d", CultureInfo.InvariantCulture)}," +
                                $"{item.JobExperience}," +
                                $"{item.Wage}," +
                                $"{item.Rank}");
            }
        }
    }
}