using System.Collections.Generic;

namespace FileCabinetApp
{
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> _validators;

        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            _validators = (List<IRecordValidator>) validators;
        }

        public void Validate(FileCabinetRecord record)
        {
            foreach (var validator in _validators)
            {
                validator.Validate(record);
            }
        }
    }
}