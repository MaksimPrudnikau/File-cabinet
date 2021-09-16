﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

[assembly:CLSCompliant(true)]
[assembly:NeutralResourcesLanguage("en")]
namespace FileCabinetApp
{
    public static class Program
    {
        private static bool _isRunning = true;
        private static IFileCabinetService _validationService;

        private static readonly Dictionary<string, Action<string>> Commands = new()
        {
            {"help", PrintHelp},
            {"exit", Exit},
            {"stat", Stat},
            {"create", Create},
            {"list", List},
            {"edit", Edit},
            {"find", Find}
        };

        public static void Main(string[] args)
        {
            try
            {
                args ??= Array.Empty<string>();
                _validationService = SetValidationRule(args);
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                Exit(string.Empty);
            }

            Console.WriteLine(EnglishSource.developed_by, FileCabinetConsts.DeveloperName); 
            Console.WriteLine(EnglishSource.hint);
            
            while (_isRunning)
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

        private static FileCabinetService SetValidationRule(IReadOnlyList<string> args)
        {
            var validationRules = args.Count switch
            {
                0 => "default",
                
                1 => args[FileCabinetConsts.ParameterIndexFullForm].ToUpperInvariant() switch
                {
                    FileCabinetConsts.DefaultValidationRuleFullForm => "default",
                    FileCabinetConsts.CustomValidationRuleFullForm => "custom",
                    
                    _ => throw new ArgumentException("No such command parameter")
                },
                
                2 => args[FileCabinetConsts.ParameterIndexShortForm].ToUpperInvariant() switch
                {
                    "DEFAULT" => "default",
                    "CUSTOM" => "custom",
                    
                    _ => throw new ArgumentException("No such command parameter")
                },
                
                _ => throw new ArgumentException("Too much command parameters")
            };

            Console.WriteLine(EnglishSource.validation_rules, validationRules);

            if (validationRules == "custom")
            {
                return new FileCabinetService(new CustomValidator());
            }

            return new FileCabinetService(new DefaultValidator());
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
            Console.WriteLine(EnglishSource.stat, FileCabinetService.Stat);
        }

        /// <summary>
        /// Create a new record according to data user entered
        /// </summary>
        private static void Create(string parameters)
        {
            try
            {
                var parameter = _validationService.ReadParameters();
                
                Console.WriteLine(EnglishSource.create, _validationService.CreateRecord(parameter));
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
            PrintFileCabinetRecordArray(FileCabinetService.GetRecords());
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
                record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
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
                Console.Error.WriteLine(EnglishSource.id_is_not_an_integer);
                return;
            }

            try
            { 
                var inputParameters = _validationService.ReadParameters(id);
                
                _validationService.EditRecord(inputParameters);
                
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
                    PrintRecord(record);
                }
            }
            catch (ArgumentException exception)
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
            var records = attribute.ToUpperInvariant() switch
            {
                "FIRSTNAME" => FileCabinetService.FindByFirstName(searchValue),
                "LASTNAME" => FileCabinetService.FindByLastName(searchValue),
                "DATEOFBIRTH" => FileCabinetService.FindByDateOfBirth(searchValue),
                _ => throw new ArgumentException("Entered attribute is not exist")
            };

            return records;
        }
        
        private static void Exit(string parameters)
        {
            Console.WriteLine(EnglishSource.exit);
            _isRunning = false;
        }
    }
}
