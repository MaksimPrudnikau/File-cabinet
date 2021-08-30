using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new();

        /// <summary>
        /// The method create new record from input data and return its id
        /// </summary>
        /// <param name="firstName"> the first name</param>
        /// <param name="lastName"> the last name</param>
        /// <param name="dateOfBirth"> date of birth in format mm/dd/yyyy</param>
        /// <param name="jobExperience"> job experience in years</param>
        /// <param name="wage"> wage in dollars</param>
        /// <param name="rank"> employee rank from F to A </param>
        /// <returns>An id of current record</returns>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, string jobExperience, string wage, char rank)
        {
            const short minimalJobExperience = 0;
            const decimal minimalWage = (decimal) 250.0;
            const char minimalRank = 'F';
            
            var record = new FileCabinetRecord
            {
                Id = list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                
                JobExperience = string.IsNullOrEmpty(jobExperience) 
                    ? minimalJobExperience 
                    : short.Parse(jobExperience, CultureInfo.InvariantCulture),
                
                Wage = string.IsNullOrEmpty(wage) 
                    ? minimalWage :
                    decimal.Parse(wage, CultureInfo.InvariantCulture),
                
                Rank = rank == default
                ? minimalRank
                : rank,
            };

            list.Add(record);

            return record.Id;
        }

        /// <summary>
        /// Return a copy of internal service`s list 
        /// </summary>
        /// <returns><see cref="list"/> converted to char array</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return list.ToArray();
        }

        /// <summary>
        /// Returns number of records that the service stores
        /// </summary>
        /// <value>An ordinal number of the last record</value>
        public int Stat => list.Count;
    }
}