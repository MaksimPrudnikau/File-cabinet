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
        /// <exception cref="ArgumentException"> either firstName or lastName are incorrect <see cref="InputName"/>.
        ///  Date of birth is incorrect <see cref="InputDateOfBirth"/>.
        ///  Job experience is incorrect <see cref="InputJobExperience"/>.
        ///  Wage is incorrect <see cref="InputWage"/>.
        ///  Rank is incorrect <see cref="InputRank"/></exception>
        /// <returns>An id of current record</returns>
        public int CreateRecord()
        {
            Console.Write("First name: ");
            var firstName = InputName();
            
            Console.Write("Last name: ");
            var lastName = InputName();
            
            var dateOfBirth = InputDateOfBirth();
            var jobExperience = InputJobExperience();
            var wage =  InputWage();
            var rank = InputRank();
            
            list.Add(new FileCabinetRecord
            {
                Id = list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                JobExperience = jobExperience,
                Wage = wage,
                Rank = rank
            });
            
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

        private static string InputName()
        {
            return InputData(GetNameFromKeyboard);
        }

        /// <summary>
        /// Get name from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Name is null or whitespace or it`s length is less than 2 or greater than 60.</exception>
        private static string GetNameFromKeyboard()
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
        private static DateTime InputDateOfBirth()
        {
            return InputData(GetDateOfBirthFromKeyboard);
        } 
        
        /// <summary>
        /// Get date of birth from keyboard and parse it from dd/mm/yyyy format to <see cref="DateTime"/>
        /// </summary>
        /// <exception cref="ArgumentException">Date of birth is null.
        /// Date of birth is not in dd/mm/yyyy format.
        /// Date of birth is less than 01-Jan-1950 or greater than current date time.</exception>
        private static DateTime GetDateOfBirthFromKeyboard()
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
        private static short InputJobExperience()
        {
            return InputData(GetJobExperienceFromKeyboard);
        }

        /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        private static short GetJobExperienceFromKeyboard()
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
        private static decimal InputWage()
        {
            return InputData(GetWageFromKeyboard);
        }

        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        private static decimal GetWageFromKeyboard()
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
        private static char InputRank()
        {
            return InputData(GetRankFromKeyboard);
        }

        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        private static char GetRankFromKeyboard()
        {
            const char defaultRank = 'F';
            char[] grades = {'F', 'D', 'C', 'B', 'A'}; // in ascending order 
            
            Console.Write("Rank: ");
            var rank = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(rank))
            {
                return defaultRank;
            }
            
            if (Array.IndexOf(grades, rank) == -1)
            {
                throw new ArgumentException("Rank is not defined in current rank system [F..A]");
            }

            return rank![0];
        }

        public void EditRecord(
            int id,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            short jobExperience,
            decimal wage,
            char rank)
        {
            list[id].FirstName = firstName;
            list[id].LastName = lastName;
            list[id].DateOfBirth = dateOfBirth;
            list[id].JobExperience = jobExperience;
            list[id].Wage = wage;
            list[id].Rank = rank;
        }
    }
}