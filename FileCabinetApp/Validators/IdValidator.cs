using System;

namespace FileCabinetApp
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
                throw new ArgumentException(RecordValidatorConsts.IdIsLessThenZero);
            }
        }
    }
}