using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService : IFileCabinetService
    {
        private readonly IRecordValidator _validator;
        private const string InputFormat = "dd/MM/yyyy";
        private static readonly List<FileCabinetRecord> List = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> FirstNameDictionary = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> LastNameDictionary = new();
        private static readonly Dictionary<DateTime, List<FileCabinetRecord>> DateOfBirthDictionary = new();

        public FileCabinetService(IRecordValidator validator)
        {
            this._validator = validator;
        }

        /// <summary>
        /// Create <see cref="Parameter"/> object from data entered from keyboard
        /// </summary>
        /// <returns></returns>
        public static Parameter InputParameters(int id = -1)
        {
            Console.Write(EnglishSource.first_name);
            var firstName = Console.ReadLine();

            Console.Write(EnglishSource.last_name);
            var lastName = Console.ReadLine();
            
            Console.Write(EnglishSource.date_of_birth);
            var dateOfBirth = Console.ReadLine();
            
            Console.Write(EnglishSource.job_experience);
            var jobExperience = Console.ReadLine();
            
            Console.Write(EnglishSource.wage);
            var wage = Console.ReadLine();
            
            Console.Write(EnglishSource.rank);
            var rank = Console.ReadLine();

            return new Parameter(id == -1 ? Stat + 1 : id, firstName, lastName, dateOfBirth, jobExperience, wage, rank);
        }

        /// <summary>
        /// The method create new record from input data and return its id
        /// </summary>
        /// <returns>An id of current record</returns>
        public int CreateRecord(Parameter parameters)
        {
            _validator.ValidateParameters(parameters);
            
            List.Add(new FileCabinetRecord
            {
                Id = parameters.Id,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = DateTime.ParseExact(parameters.DateOfBirth, InputFormat, CultureInfo.InvariantCulture),
                JobExperience = short.Parse(parameters.JobExperience, CultureInfo.InvariantCulture),
                Wage = decimal.Parse(parameters.Wage, CultureInfo.InvariantCulture),
                Rank = char.Parse(parameters.Rank)
            });

            AppendToAllDictionaries(List[^1]);
            return List[^1].Id;
        }

        /// <summary>
        /// Return a copy of internal service`s list 
        /// </summary>
        /// <returns><see cref="List"/> converted to char array</returns>
        public static IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return List.ToArray();
        }

        /// <summary>
        /// Returns number of records that the service stores
        /// </summary>
        /// <value>An ordinal number of the last record</value>
        public static int Stat => List.Count;
        
        public void EditRecord(Parameter parameters)
        {
            _validator.ValidateParameters(parameters);

            parameters.Id -= 1;
            
            if (parameters.Id >= List.Count)
            {
                Console.WriteLine(EnglishSource.record_not_found, parameters.Id);
                return;
            }

            List[parameters.Id].FirstName = parameters.FirstName;
            List[parameters.Id].LastName = parameters.LastName;
            List[parameters.Id].DateOfBirth =
                DateTime.ParseExact(parameters.DateOfBirth, InputFormat, CultureInfo.InvariantCulture);
            List[parameters.Id].JobExperience = short.Parse(parameters.JobExperience, CultureInfo.InvariantCulture);
            List[parameters.Id].Wage = (decimal)double.Parse(parameters.Wage, CultureInfo.InvariantCulture);
            List[parameters.Id].Rank = parameters.Rank[0];

            RemoveFromAllDictionaries(List[parameters.Id]);

            AppendToAllDictionaries(List[parameters.Id]);
        }

        private static void AppendToAllDictionaries(FileCabinetRecord record)
        {
            AppendToFirstNameDictionary(record);
            AppendToLastNameDictionary(record);
            AppendToDateOfBirthDictionary(record);
        }

        private static void AppendToFirstNameDictionary(FileCabinetRecord record)
        {
            if (!FirstNameDictionary.ContainsKey(record.FirstName))
            {
                FirstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord> { record });
            }
            else
            {
                FirstNameDictionary[record.FirstName].Add(record);
            }
        }

        private static void AppendToLastNameDictionary(FileCabinetRecord record)
        {
            if (!LastNameDictionary.ContainsKey(record.LastName))
            {
                LastNameDictionary.Add(record.LastName, new List<FileCabinetRecord> { record });
            }
            else
            {
                LastNameDictionary[record.LastName].Add(record);
            }
        }

        private static void AppendToDateOfBirthDictionary(FileCabinetRecord record)
        {
            if (!DateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                DateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }
            else
            {
                DateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
        }

        private static void RemoveFromAllDictionaries(FileCabinetRecord record)
        {
            if (FirstNameDictionary.Count != 1)
            {
              FirstNameDictionary[record.FirstName].Remove(record);  
            }
            
            if (LastNameDictionary.Count != 1)
            {
                LastNameDictionary[record.LastName].Remove(record);  
            }
            
            if (DateOfBirthDictionary.Count != 1)
            {
                DateOfBirthDictionary[record.DateOfBirth].Remove(record);  
            }
        }

        public static IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return FirstNameDictionary[firstName].ToArray();
        }
        
        public static IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = new List<FileCabinetRecord>();
            foreach (var item in List)
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
            foreach (var item in List)
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