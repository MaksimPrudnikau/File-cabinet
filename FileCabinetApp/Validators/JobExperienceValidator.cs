using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Validators
{
    public class JobExperienceValidator : IRecordValidator
    {
        private readonly short _minimal;
        private readonly short _maximal;

        public JobExperienceValidator(short minimalJobExperience, short maximalJobExperience)
        {
            _minimal = minimalJobExperience;
            _maximal = maximalJobExperience;
        }

        /// <summary>
        /// Validate current record's job experience
        /// <param name="record">Source record to validate</param>
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is not in range of current minimum and maximum value.</exception>
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            if (record.JobExperience < _minimal)
            {
                throw new ArgumentException(RecordValidatorConsts.JobExperienceIsLessThanMinimal);
            }

            if (record.JobExperience > _maximal)
            {
                throw new ArgumentException(RecordValidatorConsts.JobExperienceIsGreaterThanMaximal);
            }
        }
    }
}