using System;

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
            
            new IdValidator()
                .Validate(record);
            new FirstNameValidator(FileCabinetConsts.MinimalNameLength, FileCabinetConsts.MaximalNameLength)
                .Validate(record);
            new LastNameValidator(FileCabinetConsts.MinimalNameLength, FileCabinetConsts.MaximalNameLength)
                .Validate(record);
            new DateOfBirthValidator(FileCabinetConsts.MinimalDateTime, FileCabinetConsts.MaximalDateTime)
                .Validate(record);
        }
    }
}