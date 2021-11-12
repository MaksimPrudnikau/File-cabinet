using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Printers
{
    public class DefaultPrinter : IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }
            
            var record = new StringBuilder();
            foreach (var item in records)
            {
                record.Append($"{item.Id}, ");
                record.Append($"{item.FirstName}, ");
                record.Append($"{item.LastName}, ");
                record.Append(
                    $"{item.DateOfBirth.ToString(FileCabinetConsts.OutputDateFormat, CultureInfo.InvariantCulture)}, ");
                record.Append($"{item.JobExperience}, ");
                record.Append($"{item.Salary}, ");
                record.Append($"{item.Rank}");

                Console.WriteLine(record);
                record.Clear();
            }
        }
    }
}