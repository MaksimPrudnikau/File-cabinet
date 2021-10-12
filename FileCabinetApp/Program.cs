using System;
using System.Collections.Generic;
using System.Resources;
using FileCabinetApp.Handlers;

[assembly:CLSCompliant(true)]
[assembly:NeutralResourcesLanguage("en")]
namespace FileCabinetApp
{
    public static class Program
    {
        public static bool IsRunning = true;
        private static IRecordValidator _validator = new DefaultValidator();
        public static IFileCabinetService Service = new FileCabinetMemoryService(_validator);

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
                
                const int parametersIndex = 0;
                var parameters = inputs.Length == 2
                    ? inputs[parametersIndex + 1] 
                    : inputs[parametersIndex];

                var commandHandler = new HelpCommandHandler();
                commandHandler.Handle(new AppCommandRequest
                {
                    Command = command,
                    Parameters = parameters
                });
            }
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            throw new NotImplementedException();
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
    }
}
