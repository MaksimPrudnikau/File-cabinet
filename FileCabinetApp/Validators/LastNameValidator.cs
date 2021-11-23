using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Validators
{
    public class LastNameValidator : IRecordValidator
    {
        private readonly long _minLength;
        private readonly long _maxLength;

        public LastNameValidator(long minLength, long maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        /// <summary>
        /// Validate current record's last name
        /// </summary>
        /// <param name="record">Source record to validate</param>
        /// <exception cref="ArgumentException">First name's length is not in range of current minimum and maximum value</exception>
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            if (string.IsNullOrWhiteSpace(record.LastName))
            {
                throw new ArgumentException(EnglishSource.The_name_is_null_or_whitespace);
            }

            if (record.LastName.Length < _minLength || record.LastName.Length > _maxLength)
            {
                throw new ArgumentException(EnglishSource.NameWrongLength);
            }

            foreach (var item in record.LastName)
            {
                if (!char.IsLetter(item))
                {
                    throw new ArgumentException(EnglishSource.The_name_contains_non_letter_characters);
                }
            }
        }
    }
}