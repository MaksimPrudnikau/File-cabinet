using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;

[assembly:CLSCompliant(true)]
[assembly:NeutralResourcesLanguage("en")]
namespace FileCabinetApp
{
    public static class Program
    {
        public static bool IsRunning = true;
        private static IRecordValidator _validator = new DefaultValidator();
        public static IFileCabinetService Service = new FileCabinetMemoryService(_validator);

        private static readonly Dictionary<string, Action<string>> Commands = new()
        {
            {"help", PrintHelp},
            {"exit", Exit},
            {"stat", Stat},
            {"create", Create},
            {"list", List},
            {"edit", Edit},
            {"find", Find},
            {"export", Export},
            {"import", Import},
            {"remove", Remove},
            {"purge", Purge}
        };

        public static void Main(string[] args)
        {
            try
            {
                args ??= Array.Empty<string>();
                SetValidationRule(args);
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                Exit(string.Empty);
            }

            Console.WriteLine($@"Service = {Service}, Validator = {_validator}");
            Console.WriteLine(EnglishSource.developed_by, FileCabinetConsts.DeveloperName); 
            Console.WriteLine(EnglishSource.hint);

            while (IsRunning)
            {
                Console.Write(EnglishSource.console);
                var inputs = Console.ReadLine()?.Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs![commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.Error.WriteLine(EnglishSource.hint);
                    continue;
                }
                
                if (Commands.ContainsKey(command))
                {
                    const int parametersIndex = 0;
                    var parameters = inputs.Length == 2
                        ? inputs[parametersIndex + 1] 
                        : inputs[parametersIndex];
                    
                    Commands[command](parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
        }

        /// <summary>
        /// Create <see cref="IFileCabinetService"/> object according to entered command parameter
        /// </summary>
        /// <param name="args">Source command parameter</param>
        /// <exception cref="ArgumentException">Thrown when there is no such command parameter, or it is not exist.</exception>
        /// <returns></returns>
        private static void SetValidationRule(IReadOnlyList<string> args)
        {
            var commandLine = string.Concat(args);
            if (HasCommand(commandLine, FileCabinetConsts.CustomValidationRuleFullForm)
                || HasCommand(commandLine, FileCabinetConsts.CustomValidationRuleShortForm))
            {
                _validator = new CustomValidator();
            }
   
            if (HasCommand(commandLine, FileCabinetConsts.ServiceStorageFileFullForm)
            || HasCommand(commandLine, FileCabinetConsts.ServiceStorageFileShortForm))
            {
                Service = new FileCabinetFilesystemService(null, _validator);
            }
        }

        private static bool HasCommand(string commandLine, string command)
        {
            return commandLine.Contains(command, StringComparison.InvariantCultureIgnoreCase);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.Error.WriteLine(EnglishSource.no_command, command);
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            var index = Array.FindIndex(FileCabinetConsts.HelpMessages, 0, FileCabinetConsts.HelpMessages.Length,
                i => string.Equals(i[FileCabinetConsts.CommandHelpIndex ], parameters, StringComparison.OrdinalIgnoreCase));
            
            Console.Error.WriteLine(index >= 0
                ? FileCabinetConsts.HelpMessages[index][FileCabinetConsts.ExplanationHelpIndex]
                : $"There is no explanation for '{parameters}' command.");
        }
        
        /// <summary>
        /// Prints the amount of records
        /// </summary>
        private static void Stat(string parameters)
        {
            var stat = Service.GetStat();
            Console.WriteLine(EnglishSource.stat, stat.Count, stat.Deleted);
        }

        /// <summary>
        /// Create a new record according to data user entered
        /// </summary>
        private static void Create(string parameters)
        {
            try
            {
                var parameter = Service.ReadParameters();
                
                Console.WriteLine(EnglishSource.create, Service.CreateRecord(parameter));
            }
            catch (Exception exception) when(exception is ArgumentException or ArgumentNullException)
            {
                Console.Error.WriteLine(exception.Message);
                Create(parameters);
            }
        }   

        /// <summary>
        /// Return list of records added to service
        /// </summary>
        private static void List(string parameters)
        {
            foreach (var item in Service.GetRecords())
            {
                item.Print();
            }
        }

        /// <summary>
        /// Edit the record with entered id
        /// </summary>
        /// <param name="parameters">Id of the record to edit</param>
        private static void Edit(string parameters)
        {
            if (!int.TryParse(parameters, out var id))
            {
                Console.Error.WriteLine(EnglishSource.id_is_not_an_integer);
                return;
            }

            try
            { 
                var inputParameters = Service.ReadParameters(id);
                
                Service.EditRecord(inputParameters);
                
                Console.WriteLine(EnglishSource.update, id);
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }
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
                    record.Print();
                }
            }
            catch (Exception exception) when (exception is ArgumentException or FormatException)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Create an array where any attribute element equals searchValue
        /// </summary>
        /// <param name="attribute">Search property</param>
        /// <param name="searchValue">Value to search</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Entered attribute is not exist</exception>
        private static IEnumerable<FileCabinetRecord> FindByAttribute(string attribute, string searchValue)
        {
            return attribute.ToUpperInvariant() switch
            {
                "FIRSTNAME" => Service.FindByFirstName(searchValue),
                "LASTNAME" => Service.FindByLastName(searchValue),
                "DATEOFBIRTH" => Service.FindByDateOfBirth(searchValue),
                _ => throw new ArgumentException("Entered attribute is not exist")
            };
        }
        
        private static void Exit(string parameters)
        {
            Console.WriteLine(EnglishSource.exit);
            IsRunning = false;
        }

        /// <summary>
        /// Serialize all records in file with entered format
        /// </summary>
        /// <param name="parameters">Output file format</param>
        private static void Export(string parameters)
        {
            var parametersSplited = parameters.Split(' ');
            
            if (parametersSplited.Length != 2)
            {
                Console.Error.WriteLine("Wrong data format");
                return;
            }
            
            var exportFormat = parametersSplited[0];
            var directory =  parametersSplited[1];
            
            if (File.Exists(directory))
            {
                Console.Error.WriteLine(EnglishSource.Export_File_is_exist, directory);
                
                switch (Console.ReadLine())
                {
                    case "Y":
                        break;
                    case "n":
                        Console.WriteLine();
                        return;
                    default:
                        Console.Error.WriteLine("Answer must be either Y or n");
                        return;
                }
            }

            using (var file = new StreamWriter(directory))
            {
                if (!File.Exists(directory))
                {
                    Console.Error.WriteLine($"Export failed: can`t open file {directory}");
                }
                
                var snapshot = FileCabinetMemoryService.MakeSnapshot();

                switch (exportFormat)
                {
                    case "csv":
                        snapshot.SaveToCsv(file);
                        break;
                    case "xml":
                        snapshot.SaveToXml(file);
                        break;
                }
                
                file.Close();
            }
            
            Console.WriteLine(EnglishSource.All_records_are_exported_to_file, directory);
        }

        private static void Import(string parameters)
        {
            var parametersSplited = parameters.Split(' ');

            if (parametersSplited.Length != 2)
            {
                Console.Error.WriteLine("Wrong data format");
                return;
            }

            var exportFormat = parametersSplited[0];
            var directory = parametersSplited[1];

            if (!File.Exists(directory))
            {
                Console.Error.WriteLine($"File {directory} is not exist");
            }

            var snapshot = new FileCabinetServiceSnapshot();

            using var file = new StreamReader(File.OpenRead(directory));

            try
            {
                switch (exportFormat)
                {
                    case "csv":
                        snapshot.LoadFromCsv(file);
                        break;
                    case "xml":
                        snapshot.LoadFromXml(file);
                        break;
                }

                Service.Restore(snapshot);
            }
            catch (InvalidOperationException exception)
            {
                Console.Error.WriteLine(
                    $"Error: cant deserialize {directory}! {exception.Message}: {exception.InnerException?.Message}");
            }
        }

        private static void Remove(string parameters)
        {
            try
            {
                var id = Convert.ToInt32(parameters, CultureInfo.InvariantCulture);
                
                Service.Remove(id);

                Console.WriteLine(EnglishSource.Record_is_removed, id);
            }
            catch (Exception exception) when (exception is OverflowException or FormatException or ArgumentException)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }

        private static void Purge(string parameters)
        {
            try
            { 
                Service.Purge();
            }
            catch (ArgumentNullException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
