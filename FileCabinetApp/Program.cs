using System;
using System.Collections.Generic;
using System.Resources;

[assembly:CLSCompliant(true)]
[assembly:NeutralResourcesLanguage("en")]
namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Maksim Prudnikau";
        private const int CommandHelpIndex = 0;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;
        private static readonly FileCabinetService fileCabinetService = new();

        private static readonly Dictionary<string, Action<string>> commands = new()
        {
            {"help", PrintHelp},
            {"exit", Exit},
            {"stat", Stat},
            {"create", Create},
            {"list", List},
            {"edit", Edit},
            {"find", Find}
        };

        private static readonly string[][] helpMessages = {
            new[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main()
        {
            Console.WriteLine(EnglishSource.developed_by, DeveloperName);
            Console.WriteLine(EnglishSource.hint);
            Console.WriteLine();

            do
            {
                Console.Write(EnglishSource.console);
                var inputs = Console.ReadLine()?.Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs![commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(EnglishSource.hint);
                    continue;
                }
                
                if (commands.ContainsKey(command))
                {
                    const int parametersIndex = 0;
                    var parameters = inputs.Length == 2
                        ? inputs[parametersIndex + 1] 
                        : inputs[parametersIndex];
                    
                    commands[command](parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine(EnglishSource.no_command, command);
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            var index = Array.FindIndex(helpMessages, 0, helpMessages.Length,
                i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
            
            Console.WriteLine(index >= 0
                ? helpMessages[index][ExplanationHelpIndex]
                : $"There is no explanation for '{parameters}' command.");
        }
        
        /// <summary>
        /// Prints the amount of records
        /// </summary>
        private static void Stat(string parameters)
        {
            Console.WriteLine(EnglishSource.stat, fileCabinetService.Stat);
        }

        /// <summary>
        /// Create a new record according to data user entered
        /// </summary>
        private static void Create(string parameters)
        {
            Console.WriteLine(EnglishSource.create, fileCabinetService.CreateRecord());
        }
        
        /// <summary>
        /// Return list of records added to service
        /// </summary>
        private static void List(string parameters)
        {
            PrintFileCabinetRecordArray(fileCabinetService.GetRecords());
        }

        /// <summary>
        /// Prints <see cref="FileCabinetRecord"/> array
        /// </summary>
        /// <param name="source">Source array</param>
        private static void PrintFileCabinetRecordArray(IEnumerable<FileCabinetRecord> source)
        {
            foreach (var item in source)
            {
                PrintRecord(item);
            }
        }

        /// <summary>
        /// Prints record into console
        /// </summary>
        /// <param name="record">Record to print</param>
        private static void PrintRecord(FileCabinetRecord record)
        {
            Console.WriteLine(EnglishSource.print_record,
                record.Id,
                record.FirstName,
                record.LastName,
                record.DateOfBirth,
                record.JobExperience,
                record.Wage,
                record.Rank);
        }

        /// <summary>
        /// Edit the record with entered id
        /// </summary>
        /// <param name="parameters">Id of the record to edit</param>
        private static void Edit(string parameters)
        {
            if (!int.TryParse(parameters, out var id))
            {
                Console.WriteLine(EnglishSource.id_is_not_an_integer);
                return;
            }
            
            fileCabinetService.EditRecord(id - 1);
            Console.WriteLine(EnglishSource.update, id);
        }

        /// <summary>
        /// Prints all the records with entered attribute equals searchValue
        /// </summary>
        /// <param name="parameters">Parameter in format "attribute searchValue"</param>
        private static void Find(string parameters)
        {
            const int attributeIndex = 0;
            const int searchValueIndex = 1;
            var inputs = parameters.Split(' ', 2);
            var attribute = inputs[attributeIndex];
            var searchValue = inputs[searchValueIndex];

            try
            {
                foreach (var record in FindByAttribute(attribute, searchValue))
                {
                    PrintRecord(record);
                }
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Create an array where any attribute element equals <see cref="searchValue"/> 
        /// </summary>
        /// <param name="attribute">Search property</param>
        /// <param name="searchValue">Value to search</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Entered attribute is not exist</exception>
        private static IEnumerable<FileCabinetRecord> FindByAttribute(string attribute, string searchValue)
        {
            var records = attribute.ToUpperInvariant() switch
            {
                "FIRSTNAME" => fileCabinetService.FindByFirstName(searchValue),
                "LASTNAME" => fileCabinetService.FindByLastName(searchValue),
                "DATEOFBIRTH" => fileCabinetService.FindByDateOfBirth(searchValue),
                _ => throw new ArgumentException("Entered attribute is not exist")
            };

            return records;
        }
        
        private static void Exit(string parameters)
        {
            Console.WriteLine(EnglishSource.exit);
            isRunning = false;
        }
    }
}
