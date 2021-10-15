using System;

namespace FileCabinetApp
{
    public class CustomJobExperienceValidator : IRecordValidator
    {
        /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        public static ValidationResult Validate(short jobExperience)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{jobExperience}"};

            if (jobExperience < FileCabinetConsts.MinimalJobExperience)
            {
                result.Message = RecordValidatorConsts.JobExperienceIsLessThanMinimal;
                return result;
            }

            if (jobExperience > FileCabinetConsts.MaximalJobExperience)
            {
                result.Message = RecordValidatorConsts.JobExperienceIsGreaterThanMaximal;
                return result;
            }

            result.Parsed = true;
            return result;
        }

        public void Validate(FileCabinetRecord record)
        {
            if (record.JobExperience < FileCabinetConsts.MinimalJobExperience)
            {
                throw new ArgumentException(RecordValidatorConsts.JobExperienceIsLessThanMinimal);
            }

            if (record.JobExperience > FileCabinetConsts.MaximalJobExperience)
            {
                throw new ArgumentException(RecordValidatorConsts.JobExperienceIsGreaterThanMaximal);
            }
        }
    }
}