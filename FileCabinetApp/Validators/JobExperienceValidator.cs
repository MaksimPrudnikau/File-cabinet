using System;

namespace FileCabinetApp
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
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        public ValidationResult Validate(short jobExperience)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{jobExperience}"};

            if (jobExperience < _minimal)
            {
                result.Message = RecordValidatorConsts.JobExperienceIsLessThanMinimal;
                return result;
            }

            if (jobExperience > _maximal)
            {
                result.Message = RecordValidatorConsts.JobExperienceIsGreaterThanMaximal;
                return result;
            }

            result.Parsed = true;
            return result;
        }

        public void Validate(FileCabinetRecord record)
        {
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