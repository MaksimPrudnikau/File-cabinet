using System;
using System.ComponentModel;
using System.Globalization;

[assembly:CLSCompliant(true)]
namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Maksim Prudnikau";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;
        private static readonly FileCabinetService fileCabinetService = new();

        private static readonly Tuple<string, Action<string>>[] commands = {
            new("help", PrintHelp),
            new("exit", Exit),
            new("stat", Stat),
            new("create", Create),
            new("list", List),
            new ("edit", Edit)
        };

        private static readonly string[][] helpMessages = {
            new[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main()
        {
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine()?.Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs![commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 0;
                    var parameters = inputs.Length >= 1 
                        ? inputs[parametersIndex] 
                        : string.Empty;
                    commands[index].Item2(parameters);
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
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                Console.WriteLine(index >= 0
                    ? helpMessages[index][ExplanationHelpIndex]
                    : $"There is no explanation for '{parameters}' command.");
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
        
        /// <summary>
        /// Prints the amount of records
        /// </summary>
        private static void Stat(string parameters)
        {
            Console.WriteLine($"{fileCabinetService.Stat} record(s).");
        }

        /// <summary>
        /// Create a new record according to data user entered
        /// </summary>
        private static void Create(string parameters)
        {
            Console.WriteLine($"Record #{fileCabinetService.CreateRecord()} is created");
        }
        
        /// <summary>
        /// Return list of records added to service
        /// </summary>
        private static void List(string parameters)
        {
            foreach (var record in fileCabinetService.GetRecords())
            {
                PrintRecord(record);
            }
        }

        /// <summary>
        /// Prints record into console
        /// </summary>
        /// <param name="record">Record to print</param>
        private static void PrintRecord(FileCabinetRecord record)
        {
            Console.WriteLine(
                $"#{record.Id}," +
                $" {record.FirstName}," +
                $" {record.LastName}," +
                $" {record.DateOfBirth:yyyy-MMMM-dd}," +
                $" {record.JobExperience}," +
                $" {record.Wage}," +
                $" {record.Rank}");
        }

        private static void Edit(string parameters)
        {
            
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}
