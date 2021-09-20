using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService : IFileCabinetService
    {
        private static IRecordValidator _validator;
        private static readonly List<FileCabinetRecord> List = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> FirstNameDictionary = new();
        private static readonly Dictionary<string, List<FileCabinetRecord>> LastNameDictionary = new();
        private static readonly Dictionary<DateTime, List<FileCabinetRecord>> DateOfBirthDictionary = new();

        public FileCabinetService(IRecordValidator validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// The method create new record from input data and return its id
        /// </summary>
        /// <returns>An id of current record</returns>
        public int CreateRecord(Parameter parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            List.Add(new FileCabinetRecord
            {
                Id = Stat + 1,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = parameters!.DateOfBirth,
                JobExperience = parameters.JobExperience,
                Wage = parameters.Wage,
                Rank = parameters.Rank
            });
            
            AppendToAllDictionaries(List[^1]);
            
            return List[^1].Id;
        }

        /// <summary>
        /// Read parameters from keyboard and parse it to <see cref="Parameter"/> object
        /// </summary>
        /// <param name="id">Source id of read parameter</param>
        /// <returns><see cref="Parameter"/> object equivalent for read parameters</returns>
        public Parameter ReadParameters(int id = -1)
        {   
            var record = new Parameter
            {
                Id = id == -1 ? Stat + 1 : id,
                JobExperience = FileCabinetConsts.minimalJobExperience,
                Wage = FileCabinetConsts.minimalWage,
                Rank = FileCabinetConsts.minimalRank
            };

            Console.Write(EnglishSource.first_name);
            record.FirstName = ReadInput(InputConverter.NameConverter, _validator.NameValidator);
            
            Console.Write(EnglishSource.last_name);
            record.LastName = ReadInput(InputConverter.NameConverter, _validator.NameValidator);
            
            Console.Write(EnglishSource.date_of_birth);
            record.DateOfBirth = ReadInput(InputConverter.DateOfBirthConverter, _validator.DateOfBirthValidator);

            if (_validator is not CustomValidator)
            {
                return record;
            }
            
            Console.Write(EnglishSource.job_experience);
            record.JobExperience = ReadInput(InputConverter.JobExperienceConverter,
                _validator.JobExperienceValidator);
                
            Console.Write(EnglishSource.wage);
            record.Wage = ReadInput(InputConverter.WageConverter, _validator.WageValidator);
                
            Console.Write(EnglishSource.rank);
            record.Rank = ReadInput(InputConverter.RankConverter, _validator.RankValidator);

            return record;
        }

        /// <summary>
        /// Read data from keyboard, convert it by source converter and validate with source validator
        /// </summary>
        /// <typeparam name="T">Type of read data</typeparam>
        /// <param name="converter">Source converter</param>
        /// <param name="validator">Source validator</param>
        /// <returns>Correct input object</returns>
        private static T ReadInput<T>(Func<string, ConversionResult<T>> converter, Func<T, ValidationResult> validator)
        {
            do
            {
                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Parsed)
                {
                    Console.WriteLine(EnglishSource.ReadInput_Conversion_failed, conversionResult.StringRepresentation);
                    continue;
                }

                var value = conversionResult.Result;

                var validationResult = validator(value);
                if (validationResult.Parsed)
                {
                    return value;
                }
                
                Console.WriteLine(EnglishSource.Validation_failed, validationResult.StringRepresentation);
            }
            while (true);
        }
        
        /// <summary>
        /// Prints <see cref="FileCabinetRecord"/> array
        /// </summary>
        public void PrintRecords()
        {
            foreach (var item in List)
            {
                item.Print();
            }
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
        
        /// <summary>
        /// Edit record with the source one
        /// </summary>
        /// <param name="parameters">Parameter contains new data</param>
        public void EditRecord(Parameter parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            parameters.Id -= 1;
            
            RemoveFromAllDictionaries(List[parameters.Id]);
            
            List[parameters.Id].FirstName = parameters.FirstName;
            List[parameters.Id].LastName = parameters.LastName;
            List[parameters.Id].DateOfBirth = parameters.DateOfBirth;
            List[parameters.Id].JobExperience = parameters.JobExperience;
            List[parameters.Id].Wage = parameters.Wage;
            List[parameters.Id].Rank = parameters.Rank;

            AppendToAllDictionaries(List[parameters.Id]);
        }

        /// <summary>
        /// Append record to all dictionaries
        /// </summary>
        /// <param name="record">Source record</param>
        private static void AppendToAllDictionaries(FileCabinetRecord record)
        {
            AppendToFirstNameDictionary(record);
            AppendToLastNameDictionary(record);
            AppendToDateOfBirthDictionary(record);
        }

        /// <summary>
        /// Append record to FirstNameDictionary
        /// </summary>
        /// <param name="record">Source record</param>
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

        /// <summary>
        /// Append record to LastNameDictionary
        /// </summary>
        /// <param name="record">Source record</param>
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

        /// <summary>
        /// Append record to DateOfBirthDictionary
        /// </summary>
        /// <param name="record">Source record</param>
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

        /// <summary>
        /// Remove record from all dictionaries
        /// </summary>
        /// <param name="record">Source record to remove</param>
        private static void RemoveFromAllDictionaries(FileCabinetRecord record)
        {
            FirstNameDictionary[record.FirstName].Remove(record);

            LastNameDictionary[record.LastName].Remove(record);

            DateOfBirthDictionary[record.DateOfBirth].Remove(record);
        }

        /// <summary>
        /// Create an array with all occurrences of searchValue with appropriate criteria
        /// </summary>
        /// <param name="searchValue">value to search</param>
        /// <param name="criteria">Criteria to search (first name, </param>
        /// <returns></returns>
        public static IReadOnlyCollection<FileCabinetRecord> Find(string searchValue, FindCriteria criteria = FindCriteria.Lastname)
        {
            const string inputDateFormat = "yyyy-MMM-dd";
            var dateTime = new DateTime();
            if (criteria is FindCriteria.DateOfBirth)
            {
                dateTime = DateTime.ParseExact(searchValue, inputDateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
            }
            
            try
            {
                return criteria switch
                {
                    FindCriteria.Firstname => FirstNameDictionary[searchValue],
                    FindCriteria.Lastname => LastNameDictionary[searchValue],
                    FindCriteria.DateOfBirth => DateOfBirthDictionary[dateTime],
                    _ => throw new ArgumentException("Wrong find criteria")
                };
            }
            catch (KeyNotFoundException)
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Create <see cref="FileCabinetServiceSnapshot"/> object with current record array
        /// </summary>
        /// <returns><see cref="FileCabinetServiceSnapshot"/> object</returns>
        public static FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(GetRecords());
        }
    }
}