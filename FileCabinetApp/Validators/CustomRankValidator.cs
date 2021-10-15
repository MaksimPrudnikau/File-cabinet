using System;

namespace FileCabinetApp
{
    public class CustomRankValidator : IRecordValidator
    {
        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        public static ValidationResult Validate(char rank)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{rank}"};

            if (Array.IndexOf(FileCabinetConsts.Grades, rank) == -1)
            {
                result.Message = RecordValidatorConsts.RankIsNotDefinedInGrades;
                return result;
            }

            result.Parsed = true;
            return result;
        }

        public void Validate(FileCabinetRecord record)
        {
            if (Array.IndexOf(FileCabinetConsts.Grades, record.Rank) == -1)
            {
                throw new ArgumentException(RecordValidatorConsts.RankIsNotDefinedInGrades);
            }
        }
    }
}