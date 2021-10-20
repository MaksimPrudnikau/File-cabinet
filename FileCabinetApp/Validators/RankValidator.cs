using System;

namespace FileCabinetApp
{
    public class RankValidator : IRecordValidator
    {
        /// <summary>
        /// Validate current record's rank
        /// </summary>
        /// <param name="record">Source record to validate</param>
        /// <exception cref="ArgumentNullException">Record is null</exception>
        /// <exception cref="ArgumentException">Record's rank is not defined in current rank system</exception>
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            if (Array.IndexOf(FileCabinetConsts.Grades, record.Rank) == -1)
            {
                throw new ArgumentException(RecordValidatorConsts.RankIsNotDefinedInGrades);
            }
        }
    }
}