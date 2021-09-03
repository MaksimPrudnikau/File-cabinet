using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new();

        /// <summary>
        /// The method create new record from input data and return its id
        /// </summary>
        /// <exception cref="ArgumentException"> either firstName or lastName are incorrect <see cref="InputFirstName"/>.
        ///  Date of birth is incorrect <see cref="InputDateOfBirth"/>.
        ///  Job experience is incorrect <see cref="InputJobExperience"/>.
        ///  Wage is incorrect <see cref="InputWage"/>.
        ///  Rank is incorrect <see cref="InputRank"/></exception>
        /// <returns>An id of current record</returns>
        public int CreateRecord()
        {
            list.Add(new FileCabinetRecord
            {
                Id = list.Count + 1,
                FirstName = InputFirstName(),
                LastName = InputLastName(),
                DateOfBirth = InputDateOfBirth(),
                JobExperience = InputJobExperience(),
                Wage = InputWage(),
                Rank = InputRank()
            });
            
            AppendToAllDictionaries(list[^1]);
            return list[^1].Id;
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

        private static T InputData<T>(Func<T> func)
        {
            var inputIsCorrect = false;
            T output = default;
            do
            {
                try
                {
                    output = func();
                    inputIsCorrect = true;
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine(exception.Message);
                }
            } while (!inputIsCorrect);

            return output;
        }

        /// <summary>
        /// Get first name from keyboard
        /// </summary>
        private string InputFirstName()
        {
            Console.Write("First name: ");
            return InputData(GetNameFromKeyboard);
        }
        
        /// <summary>
        /// Get last name from keyboard
        /// </summary>
        private string InputLastName()
        {
            Console.Write("Last name: ");
            return InputData(GetNameFromKeyboard);
        }
        
        /// <summary>
        /// Get name from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Name is null or whitespace or it`s length is less than 2 or greater than 60.</exception>
        private string GetNameFromKeyboard()
        {
            var name = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(name) || name.Length is < 2 or > 60 )
            {
                throw new ArgumentException(
                    "Input name is null or whitespace or it`s length is less than 2 or greater than 60");
            }

            return name;
        }
        
        /// <summary>
        /// Get date of birth from keyboard and parse it from dd/mm/yyyy format to <see cref="DateTime"/>
        /// </summary>
        private DateTime InputDateOfBirth()
        {
            return InputData(GetDateOfBirthFromKeyboard);
        } 
        
        /// <summary>
        /// Get date of birth from keyboard and parse it from dd/mm/yyyy format to <see cref="DateTime"/>
        /// </summary>
        /// <exception cref="ArgumentException">Date of birth is null.
        /// Date of birth is not in dd/mm/yyyy format.
        /// Date of birth is less than 01-Jan-1950 or greater than current date time.</exception>
        private DateTime GetDateOfBirthFromKeyboard()
        {
            const string inputFormat = "dd/MM/yyyy";
            
            Console.Write("Date of birth: ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Date birth is null or whitespace");
            }
            
            if (!DateTime.TryParseExact(input!, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var dateOfBirth))
            {
                throw new ArgumentException("Date of birth is not in dd/mm/yyyy format");
            }

            var minimalDateTime = new DateTime(1950, 1, 1);
            var maximumDateTime = DateTime.Now;
            if (dateOfBirth < minimalDateTime || dateOfBirth > maximumDateTime)
            {
                throw new ArgumentException("Date of birth is less than 01-Jan-1950 or greater than current date time");
            }

            return dateOfBirth;
        }
        
        /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        private short InputJobExperience()
        {
            return InputData(GetJobExperienceFromKeyboard);
        }

        /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        private short GetJobExperienceFromKeyboard()
        {
            Console.Write("Job experience: ");
            var input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            if (!short.TryParse(input, out var jobExperience))
            {
                throw new ArgumentException(
                    "Job experience is not an integer or less than zero or greater than short.MaxValue");
            }

            if (jobExperience is < 0 or > 100)
            {
                throw new ArgumentException("Job experience is less than zero or greater than 100");
            }

            return jobExperience;
        }

        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        private decimal InputWage()
        {
            return InputData(GetWageFromKeyboard);
        }

        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        private decimal GetWageFromKeyboard()
        {
            const decimal defaultWage = (decimal)250.0;
            
            Console.Write("Wage: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultWage;
            }
            

            if (!decimal.TryParse(input, out var wage))
            {
                throw new ArgumentException(
                    "Wage is not an integer or greater than decimal.MaxValue");
            }

            if (wage < 0)
            {
                throw new ArgumentException("Wage is less than zero");
            }

            return wage;
        }

        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        private char InputRank()
        {
            return InputData(GetRankFromKeyboard);
        }

        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        private char GetRankFromKeyboard()
        {
            const char defaultRank = 'F';
            char[] grades = {'F', 'D', 'C', 'B', 'A'}; // in ascending order 
            
            Console.Write("Rank: ");
            var rank = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(rank))
            {
                return defaultRank;
            }
            
            if (rank.Length != 1 || Array.IndexOf(grades, rank[0]) == -1)
            {
                throw new ArgumentException("Rank is not defined in current rank system [F..A]");
            }

            return rank![0];
        }

        private void AppendToAllDictionaries(FileCabinetRecord record)
        {
            AppendToFirstNameDictionary(record);
            AppendToLastNameDictionary(record);
            AppendToDateOfBirthDicrionary(record);
        }

        private void AppendToFirstNameDictionary(FileCabinetRecord record)
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

        private void AppendToLastNameDictionary(FileCabinetRecord record)
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

        private void AppendToDateOfBirthDicrionary(FileCabinetRecord record)
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

        private void RemoveFromAllDictionaries(FileCabinetRecord record)
        {
            firstNameDictionary[record.FirstName].Remove(record);
            lastNameDictionary[record.LastName].Remove(record);
            dateOfBirthDictionary[record.DateOfBirth].Remove(record);
        }

        public void EditRecord(int id)
        {
            if (id >= list.Count)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }
            
            RemoveFromAllDictionaries(list[id]);

            list[id].FirstName = InputFirstName();
            list[id].LastName = InputLastName();
            list[id].DateOfBirth = InputDateOfBirth();
            list[id].JobExperience = InputJobExperience();
            list[id].Wage = InputWage();
            list[id].Rank = InputRank();
            
            AppendToAllDictionaries(list[id]);
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return firstNameDictionary[firstName].ToArray();
        }
        
        public FileCabinetRecord[] FindByLastName(string lastName)
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
        
        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
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