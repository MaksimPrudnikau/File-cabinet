using System;

namespace FileCabinetGenerator
{
    public static class OptionsValidator
    {
        /// <summary>
        /// Validate <see cref="Options"/> parameter
        /// </summary>
        /// <param name="options">Source options</param>
        /// <exception cref="ArgumentException"> Records amount is less than zero or greater than int.MaxValue </exception>
        /// <exception cref="ArgumentException"> Start id is less than zero </exception>
        public static void Validate(Options options)
        {
            StartIdValidator(options.StartId);
            RecordsAmountValidator(options.Count);
        }

        private static void StartIdValidator(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Start id is less than zero");
            }
        }

        private static void RecordsAmountValidator(long amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Records amount is less than zero");
            }

            if (amount > int.MaxValue)
            {
                throw new ArgumentException("Records amount is greater than int.MaxValue");
            }
        }
    }
}