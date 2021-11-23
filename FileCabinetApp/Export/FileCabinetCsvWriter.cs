using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Export
{
    public class FIleCabinetCsvWriter
    {
        public const char Delimiter = ',';
        
        private readonly TextWriter _file;

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
        /// Serialize source records to current <see cref="TextWriter"/>
        /// </summary>
        /// <param name="record">Source records to write</param>
        public void Write(IEnumerable<FileCabinetRecord> record, char delimiter = Delimiter)
        {
            if (record is null)
            {
                return;
            }
            
            foreach (var item in record)
            {
                _file.WriteLine($"{item.Id}{delimiter}" +
                                $"{item.FirstName}{delimiter}" +
                                $"{item.LastName}{delimiter}" +
                                $"{item.DateOfBirth.ToString(FileCabinetConsts.InputDateFormat, CultureInfo.InvariantCulture)}{delimiter}" +
                                $"{item.JobExperience}{delimiter}" +
                                $"{item.Salary}{delimiter}" +
                                $"{item.Rank}");
            }
        }
    }
}