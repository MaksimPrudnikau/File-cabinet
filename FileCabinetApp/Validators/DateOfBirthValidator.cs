using System;

namespace FileCabinetApp
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime _from;
        private readonly DateTime _to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            _from = from;
            _to = to;
        }

        /// <summary>
        /// Validate current record's date of birth
        /// </summary>
        /// <param name="record">Source record to validate</param>
        /// <exception cref="ArgumentNullException">Date of birth is null or whitespace</exception>
        /// <exception cref="ArgumentException">Date of birth is not in dd/MM/yyyy format.
        /// Date of birth is not in range of current minimum and maximum value</exception>
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            var dateOfBirth = record.DateOfBirth;
            
            if (dateOfBirth < _from)
            {
                throw new ArgumentException(RecordValidatorConsts.DateOfBirthIsLessThanMinimal);
            }

            if (dateOfBirth > _to)
            {
                throw new ArgumentException(RecordValidatorConsts.DateOfBirthIsGreaterThanMaximal);
            }
        }
    }
}