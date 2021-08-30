using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new();

        /// <summary>
        /// The method create new record from input data and return its id
        /// </summary>
        /// <param name="firstName">the first name</param>
        /// <param name="lastName">the last name</param>
        /// <param name="dateOfBirth"> date of birth in format mm/dd/yyyy</param>
        /// <returns>An id of current record</returns>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth)
        {
            var record = new FileCabinetRecord
            {
                Id = list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };

            list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            // TODO: добавьте реализацию метода
            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Returns number of records that the service stores
        /// </summary>
        /// <returns>An ordinal number of the last record</returns>
        public int GetStat()
        {
            return list.Count;
        }
    }
}