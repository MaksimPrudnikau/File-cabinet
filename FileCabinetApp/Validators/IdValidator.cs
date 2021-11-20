using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Validators
{
    public class IdValidator : IRecordValidator
    {
        /// <summary>
        /// Validate current record's id
        /// </summary>
        /// <param name="record">Source record to validate</param>
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            if (record.Id < 0)
            {
                throw new ArgumentException(EnglishSource.Id_is_less_than_zero);
            }
        }
    }
}