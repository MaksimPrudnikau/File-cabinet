using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class DefaultValidator : CompositeValidator
    {
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
        }

        public DefaultValidator() : 
            base(new IRecordValidator[]
            {
                new IdValidator(),
                new FirstNameValidator(FileCabinetConsts.MinimalNameLength,FileCabinetConsts.MaximalNameLength),
                new LastNameValidator(FileCabinetConsts.MinimalNameLength,FileCabinetConsts.MaximalNameLength),
                new DateOfBirthValidator(FileCabinetConsts.MinimalDateTime, FileCabinetConsts.MaximalDateTime)
            }){}
    }
}