using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Validators
{
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> _validators;

        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            _validators = (List<IRecordValidator>) validators;
        }

        /// <summary>
        /// Validate the source record with all current validators
        /// </summary>
        /// <param name="record">Source record to validate</param>
        public void Validate(FileCabinetRecord record)
        {
            foreach (var validator in _validators)
            {
                validator.Validate(record);
            }
        }
    }
}