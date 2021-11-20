using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Validators
{
    public class RankValidator : IRecordValidator
    {
        private readonly IList<char> _grades;
        
        public RankValidator(Collection<char> grades)
        {
            _grades = grades ?? throw new ArgumentNullException(nameof(grades));
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
            
            if (_grades.IndexOf(record.Rank) == -1)
            {
                throw new ArgumentException(EnglishSource.Rank_Is_Not_Defined_In_Grades);
            }
        }
    }
}