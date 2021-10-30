using System;
using System.Collections.ObjectModel;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Validators
{
    public class RankValidator : IRecordValidator
    {
        private readonly char[] _grades;
        
        public RankValidator(Collection<char> grades)
        {
            if (grades is null)
            {
                throw new ArgumentNullException(nameof(grades));
            }

            _grades = new char[grades.Count];
            grades.CopyTo(_grades, 0);
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