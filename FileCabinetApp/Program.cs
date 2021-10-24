using System;
using System.Collections.Generic;
using System.Resources;
using FileCabinetApp.Handlers;
using FileCabinetApp.Printers;
using FileCabinetApp.ValidationRules;

[assembly:CLSCompliant(true)]
[assembly:NeutralResourcesLanguage("en")]
namespace FileCabinetApp
{
    public static class Program
    {
        private static bool _isRunning = true;

        private static IRecordValidator _validator;
        private static IFileCabinetService _service;

        public static void Main(string[] args)
        {
            Console.WriteLine($@"Service = {_service}, Validator = {_validator}");
            Console.WriteLine(EnglishSource.developed_by, FileCabinetConsts.DeveloperName); 
            Console.WriteLine(EnglishSource.hint);

            SetValidationRules(args);
            while (_isRunning)
            {
                var command = ReadUsersImport(out var parameters);
                
                if (string.IsNullOrEmpty(command))
                {
                    Console.Error.WriteLine(EnglishSource.hint);
                    continue;
                }
                
                CommandHandlerBase commandHandler;
                try
                {
                    commandHandler = CreateCommandHandlers(_service, command);
                }
                catch (ArgumentOutOfRangeException)
                {
                    PrintMissedCommandInfo(command);
                    continue;
                }
                
                commandHandler.Handle(new AppCommandRequest
                {
                    Command = command,
                    Parameters = parameters
                });
            }
        }

        private static string ReadUsersImport(out string parameters)
        {
            const int commandIndex = 0;
            const int parametersIndex = 0;
            
            Console.Write(EnglishSource.console);
            var inputs = Console.ReadLine()?.Split(' ', 2);
            
            var command = inputs![commandIndex];

            parameters = inputs.Length == 2
                ? inputs[parametersIndex + 1]
                : inputs[parametersIndex];

            return command;
        }

        private static void SetValidationRules(IEnumerable<string> args)
        {
            try
            {
                var commandLineParser = new CommandLineParser(args);
                _validator = commandLineParser.Validator;
                _service = commandLineParser.Service;
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                _isRunning = false;
            }
        }
        
        private static void PrintMissedCommandInfo(string command)
        {
            Console.Error.WriteLine(EnglishSource.no_command, command);
            Console.WriteLine();
        }

        private static CommandHandlerBase CreateCommandHandlers(IFileCabinetService service, string command)
        {
            /* TODO: Исправить на Change of Responsibility
            // var helpHandler = new HelpCommandHandler();
            // var createHandler = new CreateCommandHandler(service);
            // var editHandler = new EditCommandHandler(service);
            // var listHandler = new ListCommandHandler(service, RecordsPrinter.Default);
            // var statHandler = new StatCommandHandler(service);
            // var findHandler = new FindCommandHandler(service, RecordsPrinter.Default);
            // var exportHandler = new ExportCommandHandler();
            // var importHandler = new ImportCommandHandler(service);
            // var removeHandler = new RemoveCommandHandler(service);
            // var purgeHandler = new PurgeCommandHandler(service);
            // var exitHandler = new ExitCommandHandler();
            //
            // helpHandler.SetNext(createHandler);
            // createHandler.SetNext(editHandler);
            // editHandler.SetNext(listHandler);
            // listHandler.SetNext(statHandler);
            // statHandler.SetNext(findHandler);
            // findHandler.SetNext(exportHandler);
            // exportHandler.SetNext(importHandler);
            // importHandler.SetNext(removeHandler);
            // removeHandler.SetNext(purgeHandler);
            // purgeHandler.SetNext(exitHandler);
            */
            
            return command switch
            {
                "help" => new HelpCommandHandler(),
                "create" => new CreateCommandHandler(service),
                "edit" => new EditCommandHandler(service),
                "list" => new ListCommandHandler(service, RecordsPrinter.Default),
                "stat" => new StatCommandHandler(service),
                "find" => new FindCommandHandler(service, RecordsPrinter.Default),
                "export" => new ExportCommandHandler(),
                "import" => new ImportCommandHandler(service),
                "remove" => new RemoveCommandHandler(service),
                "purge" => new PurgeCommandHandler(service),
                "exit" => new ExitCommandHandler(x => _isRunning = x),
                _ => throw new ArgumentOutOfRangeException(nameof(command))
            };
        }
    }
}
