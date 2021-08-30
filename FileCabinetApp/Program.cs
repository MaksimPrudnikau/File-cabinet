using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

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
        private static FileCabinetService fileCabinetService = new();

        private static Tuple<string, Action<string>>[] commands = {
            new("help", PrintHelp),
            new("exit", Exit),
            new("stat", Stat),
            new("create", Create)
        };

        private static string[][] helpMessages = {
            new[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
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
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
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
            var recordsCount = fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        /// <summary>
        /// Create a new record according to data user entered
        /// </summary>
        private static void Create(string parameters)
        {
            Console.Write("First name: ");
            var firstName = Console.ReadLine();
            Console.Write("Last name: ");
            var lastName = Console.ReadLine();
            Console.Write("Date of birth: ");
            
            var dateOfBirth = Console.ReadLine()?.Split('/');

            Console.WriteLine(
                $"#Record #{fileCabinetService.CreateRecord(firstName, lastName, ConvertToDateTime(dateOfBirth))} is created", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts string in format dd/mm/yyyy to <see cref="DateTime"/>
        /// </summary>
        /// <param name="date">Array of date { day, month, year }</param>
        /// <returns>Data time parsed from input array </returns>
        private static DateTime ConvertToDateTime(IReadOnlyList<string> date)
        {
            var dayOfBirth = int.Parse(date[0], CultureInfo.InvariantCulture);
            var monthOfBirth = int.Parse(date[1], CultureInfo.InvariantCulture);
            var yearOfBirth = int.Parse(date[1], CultureInfo.InvariantCulture);
            return new DateTime(yearOfBirth, monthOfBirth, dayOfBirth);
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}
