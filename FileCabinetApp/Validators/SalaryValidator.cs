using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Validators
{
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal _minimal;
        private readonly decimal _maximal;

        public SalaryValidator(decimal minimalSalary, decimal maximalSalary)
        {
            _minimal = minimalSalary;
            _maximal = maximalSalary;
        }
        
        /// <summary>
        /// Validate current record's salary
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero or less than current minimal value</exception>
        public void Validate(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            if (record.Salary < _minimal)
            {
                throw new ArgumentException(RecordValidatorConsts.WageIsLessThanMinimal);
            }
            
            if (record.Salary > _maximal)
            {
                throw new ArgumentException(RecordValidatorConsts.WageIsGreaterThanMaximal);
            }
        }
    }
}