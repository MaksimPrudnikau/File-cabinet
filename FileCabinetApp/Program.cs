using System;
using System.Collections.Generic;
using System.Resources;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.Decorators.Logger;
using FileCabinetApp.FileCabinetService.Decorators.Meter;
using FileCabinetApp.Handlers;
using FileCabinetApp.Printers.Table;
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
            
            Console.WriteLine(EnglishSource.Service_And_Validator_info, _service, _validator);
            Console.WriteLine(EnglishSource.developed_by, FileCabinetConsts.DeveloperName); 
            Console.WriteLine(EnglishSource.hint);

            while (_isRunning)
            {
                var read = TryReadUsersImport(out var request);
                if (!read)
                {
                    continue;
                }
                
                var created = TryCreateCommandHandlers(_service, out var handler);
                if (!created)
                {
                    continue;
                }
                
                handler.Handle(request);
            }
        }

        /// <summary>
        /// Try <see cref="CreateCommandHandlers"/>
        /// </summary>
        /// <param name="service">Current service</param>
        /// <param name="handler">The first element from created chain of handlers</param>
        /// <returns>True if handlers were created successfully</returns>
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

        /// <summary>
        /// Try <see cref="ReadUsersImport"/>
        /// </summary>
        /// <param name="request">Parsed request</param>
        /// <returns>True if parsing was successful</returns>
        private static bool TryReadUsersImport(out AppCommandRequest request)
        {
            request = null;
            
            try
            {
                request = ReadUsersImport();
                return true;
            }
            catch (ArgumentException exception)
            {
                ErrorCommandHandler.Handle(exception.Source);
                return false;
            }
        }

        /// <summary>
        /// Get users command from keyboard, split it by whitespace and parse to <see cref="AppCommandRequest"/>
        /// </summary>
        /// <returns>parsed <see cref="AppCommandRequest"/> object</returns>
        /// <exception cref="ArgumentOutOfRangeException">Input command is not defined in current <see cref="RequestCommand"/></exception>
        private static AppCommandRequest ReadUsersImport()
        {
            const int commandIndex = 0;
            const int parametersIndex = 0;
            
            Console.Write(EnglishSource.console);
            var inputs = Console.ReadLine()?.Split(' ', 2);
            
            var command =  inputs![commandIndex];

            var parameters = inputs.Length == 2
                ? inputs[parametersIndex + 1]
                : string.Empty;

            var parsed = Enum.TryParse<RequestCommand>(command, true, out var request);

            if (!parsed)
            {
                throw new ArgumentException(EnglishSource.ErrorCommandHandler_Handle_Is_Not_A_Command)
                    {Source = command};
            }
            
            return new AppCommandRequest
            {
                Command = request,
                Parameters = parameters
            };
        }

        /// <summary>
        /// Parse command line arguments and create the suitable <see cref="IFileCabinetService"/> and <see cref="ValidationRules"/>
        /// </summary>
        /// <param name="args">Line arguments split by whitespace</param>
        private static void SetValidationRules(IEnumerable<string> args)
        {
            try
            {
                var commandLineParser = new CommandLineParser(args);
                _validator = commandLineParser.Validator;
                _service = commandLineParser.Service;
                _useStopwatch = commandLineParser.UseStopWatch;
                _useLogger = commandLineParser.UseLogger;

                if (_useLogger && _useStopwatch)
                {
                    throw new ArgumentException(EnglishSource.Cannot_use_both_logger_and_stopwatch);
                }
                
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

        /// <summary>
        /// Create the chain of command handlers starts from <see cref="CreateCommandHandler"/>
        /// and ends by <see cref="UpdateCommandHandler"/>
        /// </summary>
        /// <param name="service">Current service</param>
        /// <returns>The command handlers chain</returns>
        private static CommandHandlerBase CreateCommandHandlers(IFileCabinetService service)
        {
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(service);
            var statHandler = new StatCommandHandler(service);
            var exportHandler = new ExportCommandHandler(service);
            var importHandler = new ImportCommandHandler(service);
            var deleteHandler = new DeleteCommandHandler(service);
            var purgeHandler = new PurgeCommandHandler(service);
            var exitHandler = new ExitCommandHandler(x => _isRunning = x);
            var insertHandler = new InsertCommandHandler(service);
            var updateHandler = new UpdateCommandHandler(service);
            var selectHandler = new SelectCommandHandler(service, new TablePrinter());
            
            helpHandler.SetNext(createHandler);
            createHandler.SetNext(statHandler);
            statHandler.SetNext(exportHandler);
            exportHandler.SetNext(importHandler);
            importHandler.SetNext(deleteHandler);
            deleteHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(exitHandler);
            exitHandler.SetNext(insertHandler);
            insertHandler.SetNext(updateHandler);
            updateHandler.SetNext(selectHandler);
            
            return helpHandler;
        }
    }
}
