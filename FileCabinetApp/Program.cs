using System;
using System.Collections.Generic;
using System.Resources;
using FileCabinetApp.Handlers;
using FileCabinetApp.Printers;

[assembly:CLSCompliant(true)]
[assembly:NeutralResourcesLanguage("en")]
namespace FileCabinetApp
{
    public static class Program
    {
        private static bool _isRunning = true;
        // TODO: Закройте поле isRunning, сделайте Constructor Injection делегата Action<bool> в класс ExitCommandHandler

        private static IRecordValidator _validator = new ValidationBuilder().CreateDefault();
        private static IFileCabinetService _service = new FileCabinetMemoryService(_validator);

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
                return;
            }

            Console.WriteLine($@"Service = {_service}, Validator = {_validator}");
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
                
                const int parametersIndex = 0;
                var parameters = inputs.Length == 2
                    ? inputs[parametersIndex + 1] 
                    : inputs[parametersIndex];

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
                "exit" => new ExitCommandHandler(),
                _ => throw new ArgumentOutOfRangeException(nameof(command))
            };
        }

        /// <summary>
        /// Create <see cref="IFileCabinetService"/> object according to entered command parameter
        /// </summary>
        /// <param name="args">Source command parameter</param>
        /// <exception cref="ArgumentException">Thrown when there is no such command parameter, or it is not exist.</exception>
        /// <returns></returns>
        private static void SetValidationRule(IEnumerable<string> args)
        {
            var commandLine = string.Concat(args);
            if (HasCommand(commandLine, FileCabinetConsts.CustomValidationRuleFullForm)
                || HasCommand(commandLine, FileCabinetConsts.CustomValidationRuleShortForm))
            {
                _validator = new ValidationBuilder().CreateCustom();
            }
   
            if (HasCommand(commandLine, FileCabinetConsts.ServiceStorageFileFullForm)
            || HasCommand(commandLine, FileCabinetConsts.ServiceStorageFileShortForm))
            {
                _service = new FileCabinetFilesystemService(null, _validator);
            }
        }

        private static bool HasCommand(string commandLine, string command)
        {
            return commandLine.Contains(command, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
