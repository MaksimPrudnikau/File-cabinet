using System;
using System.Collections.Generic;
using System.Resources;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.Decorators.Logger;
using FileCabinetApp.FileCabinetService.Decorators.Meter;
using FileCabinetApp.Handlers;
using FileCabinetApp.Handlers.Update;
using FileCabinetApp.Printers;
using FileCabinetApp.Validation;
using FileCabinetApp.Validators;

[assembly:CLSCompliant(true)]
[assembly:NeutralResourcesLanguage("en")]
namespace FileCabinetApp
{
    public static class Program
    {
        private static IRecordValidator _validator;
        private static IFileCabinetService _service;
        
        private static bool _isRunning = true;
        private static bool _useStopwatch;
        private static bool _useLogger;

        public static void Main(string[] args)
        {
            SetValidationRules(args);
            
            Console.WriteLine($@"Service = {_service}, Validator = {_validator}");
            Console.WriteLine(EnglishSource.developed_by, FileCabinetConsts.DeveloperName); 
            Console.WriteLine(EnglishSource.hint);

            while (_isRunning)
            {
                var read = TryReadUsersImport(out var request);
                var created = TryCreateCommandHandlers(_service, out var handler);

                if (!read || !created)
                {
                    continue;
                }
                
                handler.Handle(request);
            }
        }

        private static bool TryCreateCommandHandlers(IFileCabinetService service, out CommandHandlerBase handler)
        {
            try
            {
                handler = CreateCommandHandlers(service);
            }
            catch (Exception exception) when (exception is ArgumentOutOfRangeException or NullReferenceException)
            {
                handler = null;
                return false;
            }

            return true;
        }

        private static bool TryReadUsersImport(out AppCommandRequest request)
        {
            request = null;
            
            try
            {
                request = ReadUsersImport();
                return true;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                ErrorCommandHandler.Handle($"{exception.ActualValue}");
                return false;
            }
        }

        private static AppCommandRequest ReadUsersImport()
        {
            const int commandIndex = 0;
            const int parametersIndex = 0;
            
            Console.Write(EnglishSource.console);
            var inputs = Console.ReadLine()?.Split(' ', 2);
            
            var command =  inputs![commandIndex];

            var parameters = inputs.Length == 2
                ? inputs[parametersIndex + 1]
                : inputs[parametersIndex];

            var parsed = Enum.TryParse<RequestCommand>(command, true, out var request);

            if (!parsed)
            {
                throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }
            
            return new AppCommandRequest
            {
                Command = request,
                Parameters = parameters
            };
        }

        private static void SetValidationRules(IEnumerable<string> args)
        {
            try
            {
                var commandLineParser = new CommandLineParser(args);
                _validator = commandLineParser.Validator;
                _service = commandLineParser.Service;
                _useStopwatch = commandLineParser.UseStopWatch;
                _useLogger = commandLineParser.UseLogger;
                if (_useStopwatch)
                {
                    _service = new ServiceMeter(_service);
                }
                else if (_useLogger)
                {
                    _service = new ServiceLogger(_service, FileCabinetConsts.LogsFileName);
                }
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

        private static CommandHandlerBase CreateCommandHandlers(IFileCabinetService service)
        {
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(service);
            var listHandler = new ListCommandHandler(service, RecordsPrinter.Default);
            var statHandler = new StatCommandHandler(service);
            var findHandler = new FindCommandHandler(service, RecordsPrinter.Default);
            var exportHandler = new ExportCommandHandler(service);
            var importHandler = new ImportCommandHandler(service);
            var deleteHandler = new DeleteCommandHandler(service);
            var purgeHandler = new PurgeCommandHandler(service);
            var exitHandler = new ExitCommandHandler(x => _isRunning = x);
            var insertHandler = new InsertCommandHandler(service);
            var updateHandler = new UpdateCommandHandler(service);
            
            helpHandler.SetNext(createHandler);
            createHandler.SetNext(listHandler);
            listHandler.SetNext(statHandler);
            statHandler.SetNext(findHandler);
            findHandler.SetNext(exportHandler);
            exportHandler.SetNext(importHandler);
            importHandler.SetNext(deleteHandler);
            deleteHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(exitHandler);
            exitHandler.SetNext(insertHandler);
            insertHandler.SetNext(updateHandler);

            return helpHandler;
        }
    }
}
