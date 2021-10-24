using System;

namespace FileCabinetApp
{
    public class RankValidator : IRecordValidator
    {
        private readonly char[] _grades;
        
        public RankValidator(char[] grades)
        {
            _grades = grades;
        }
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
            
            if (Array.IndexOf(_grades, record.Rank) == -1)
            {
                throw new ArgumentException(RecordValidatorConsts.RankIsNotDefinedInGrades);
            }
        }
    }
}