using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class DefaultValidator : IRecordValidator
    {
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            var results = new List<ValidationResult>
            {
                DefaultIdValidator.Validate(record.Id),
                DefaultNameValidator.Validate(record.FirstName),
                DefaultNameValidator.Validate(record.LastName),
                DefaultDateOfBirthValidator.Validate(record.DateOfBirth),
            };

            foreach (var item in results)
            {
                if (!item.Parsed)
                {
                    throw new ArgumentException(item.Message);
                }
            }
        }
    }
}