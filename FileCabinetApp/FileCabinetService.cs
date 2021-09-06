using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public abstract class FileCabinetService
    {
        private const string inputFormat = "dd/MM/yyyy";
        private static readonly List<FileCabinetRecord> list = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new();
        private static readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new();

        /// <summary>
        /// Create <see cref="Parameter"/> object from data entered from keyboard
        /// </summary>
        /// <returns></returns>
        public static Parameter InputParameters()
        {
            var parameter = new Parameter{Id = Stat + 1};

            Console.Write(EnglishSource.first_name);
            parameter.FirstName = Console.ReadLine();

            Console.Write(EnglishSource.last_name);
            parameter.LastName = Console.ReadLine();
            
            Console.Write(EnglishSource.date_of_birth);
            parameter.DateOfBirth = Console.ReadLine();
            
            Console.Write(EnglishSource.job_experience);
            parameter.JobExperience = Console.ReadLine();
            
            Console.Write(EnglishSource.wage);
            parameter.Wage = Console.ReadLine();
            
            Console.Write(EnglishSource.rank);
            parameter.Rank = Console.ReadLine();

            return parameter;
        }

        /// <summary>
        /// The method create new record from input data and return its id
        /// </summary>
        /// <returns>An id of current record</returns>
        public static int CreateRecord(Parameter parameters)
        {
            list.Add(new FileCabinetRecord
            {
                Id = parameters.Id,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = DateTime.ParseExact(parameters.DateOfBirth, inputFormat, CultureInfo.InvariantCulture),
                JobExperience = short.Parse(parameters.JobExperience, CultureInfo.InvariantCulture),
                Wage = decimal.Parse(parameters.Wage, CultureInfo.InvariantCulture),
                Rank = char.Parse(parameters.Rank)
            });
            
            AppendToAllDictionaries(list[^1]);
            return list[^1].Id;
        }

        /// <summary>
        /// Validate all parameters
        /// </summary>
        /// <param name="parameters">Entered parameters</param>
        protected virtual void ValidateParameters(Parameter parameters) { }

        /// <summary>
        /// Return a copy of internal service`s list 
        /// </summary>
        /// <returns><see cref="list"/> converted to char array</returns>
        public static IEnumerable<FileCabinetRecord> GetRecords()
        {
            return list.ToArray();
        }

        /// <summary>
        /// Returns number of records that the service stores
        /// </summary>
        /// <value>An ordinal number of the last record</value>
        public static int Stat => list.Count;
        
        public void EditRecord(Parameter parameters)
        {
            ValidateParameters(parameters);
            
            if (parameters.Id >= list.Count)
            {
                Console.WriteLine(EnglishSource.record_not_found, parameters.Id);
                return;
            }

            list[parameters.Id].FirstName = parameters.FirstName;
            list[parameters.Id].LastName = parameters.LastName;
            list[parameters.Id].DateOfBirth =
                DateTime.ParseExact(parameters.DateOfBirth, inputFormat, CultureInfo.InvariantCulture);
            list[parameters.Id].JobExperience = short.Parse(parameters.JobExperience, CultureInfo.InvariantCulture);
            list[parameters.Id].Wage = decimal.Parse(parameters.Wage, CultureInfo.InvariantCulture);
            list[parameters.Id].Wage = char.Parse(parameters.Rank);

            RemoveFromAllDictionaries(list[parameters.Id]);

            AppendToAllDictionaries(list[parameters.Id]);
        }

        private static void AppendToAllDictionaries(FileCabinetRecord record)
        {
            AppendToFirstNameDictionary(record);
            AppendToLastNameDictionary(record);
            AppendToDateOfBirthDictionary(record);
        }

        private static void AppendToFirstNameDictionary(FileCabinetRecord record)
        {
            if (!firstNameDictionary.ContainsKey(record.FirstName))
            {
                firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord> { record });
            }
            else
            {
                firstNameDictionary[record.FirstName].Add(record);
            }
        }

        private static void AppendToLastNameDictionary(FileCabinetRecord record)
        {
            if (!lastNameDictionary.ContainsKey(record.LastName))
            {
                lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord> { record });
            }
            else
            {
                lastNameDictionary[record.LastName].Add(record);
            }
        }

        private static void AppendToDateOfBirthDictionary(FileCabinetRecord record)
        {
            if (!dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }
            else
            {
                dateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
        }

        private static void RemoveFromAllDictionaries(FileCabinetRecord record)
        {
            firstNameDictionary[record.FirstName].Remove(record);
            lastNameDictionary[record.LastName].Remove(record);
            dateOfBirthDictionary[record.DateOfBirth].Remove(record);
        }
        

        public static IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return firstNameDictionary[firstName].ToArray();
        }
        
        public static IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = new List<FileCabinetRecord>();
            foreach (var item in list)
            {
                if (item.LastName == lastName)
                {
                    records.Add(item);
                }
            }

            return records.ToArray();
        }
        
        public static IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var records = new List<FileCabinetRecord>();
            foreach (var item in list)
            {
                if (item.DateOfBirth == DateTime.ParseExact(dateOfBirth, "yyyy-MMM-dd", CultureInfo.InvariantCulture))
                {
                    records.Add(item);
                }
            }

            return records.ToArray();
        }
    }
}