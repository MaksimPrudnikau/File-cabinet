using System;

namespace FileCabinetApp
{
    public class DefaultIdValidator : IRecordValidator
    {
        /// <summary>
        /// Validate source id
        /// </summary>
        /// <param name="id">Source id</param>
        public static ValidationResult Validate(int id)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = $"{id}"};

            if (id < 0)
            {
                result.Message = RecordValidatorConsts.IdIsLessThenZero;
                return result;
            }

            result.Parsed = true;
            return result;
        }

        public void Validate(FileCabinetRecord record)
        {
            if (record.Id < 0)
            {
                throw new ArgumentException(RecordValidatorConsts.IdIsLessThenZero);
            }
        }
    }
}