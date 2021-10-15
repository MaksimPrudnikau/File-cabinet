using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Handlers
{
    public class DefaultRecordPrinter : IRecordPrinter
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
                record.Append($"{item.DateOfBirth:dd/MMM/yyyy}, ");
                record.Append($"{item.JobExperience}, ");
                record.Append($"{item.Salary}, ");
                record.Append($"{item.Rank}");
                record.Clear();
            }
        }
    }
}