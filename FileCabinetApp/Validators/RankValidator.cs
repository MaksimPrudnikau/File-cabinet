using System;

namespace FileCabinetApp
{
    public class RankValidator : IRecordValidator
    {
        private static readonly char[] Grades = {'F', 'D', 'C', 'B', 'A'};
        
        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        public ValidationResult Validate(char rank)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{rank}"};

            if (Array.IndexOf(Grades, rank) == -1)
            {
                result.Message = RecordValidatorConsts.RankIsNotDefinedInGrades;
                return result;
            }

            result.Parsed = true;
            return result;
        }

        public void Validate(FileCabinetRecord record)
        {
            if (Array.IndexOf(Grades, record.Rank) == -1)
            {
                throw new ArgumentException(RecordValidatorConsts.RankIsNotDefinedInGrades);
            }
        }
    }
}