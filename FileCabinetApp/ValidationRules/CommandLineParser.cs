using System;
using System.Collections.Generic;

namespace FileCabinetApp.ValidationRules
{
    public class CommandLineParser
    {
        public IRecordValidator Validator { get; private set; }
        
        public IFileCabinetService Service { get; private set; }
        
        public bool UseStopWatch { get; private set; }
        
        public bool UseLogger { get; private set; }

        public CommandLineParser(IEnumerable<string> args)
        {
            SetValidationRule(args);
        }
        
        /// <summary>
        /// Create <see cref="IFileCabinetService"/> object according to entered command parameter
        /// </summary>
        /// <param name="args">Source command parameter</param>
        /// <exception cref="ArgumentException">Thrown when there is no such command parameter, or it is not exist.</exception>
        /// <returns></returns>
        private void SetValidationRule(IEnumerable<string> args)
        {
            var commandLine = string.Concat(args);
            var isCustom = IsCustomService(commandLine);
            
            Validator = isCustom
                ? new ValidationBuilder().CreateCustom()
                : new ValidationBuilder().CreateDefault();

            Service = IsFileSystemService(commandLine)
                ? new FileCabinetFilesystemService(null, Validator)
                : new FileCabinetMemoryService(Validator, isCustom);

            UseStopWatch = IsUseStopWatch(commandLine);
            UseLogger = IsUseLogger(commandLine);
        }

        private static bool IsCustomService(string commandLine)
        {
            return HasCommand(commandLine, FileCabinetConsts.CustomValidationRuleFullForm)
                   || HasCommand(commandLine, FileCabinetConsts.CustomValidationRuleShortForm);
        }

        private static bool IsFileSystemService(string commandLine)
        {
            return HasCommand(commandLine, FileCabinetConsts.ServiceStorageFileFullForm)
                   || HasCommand(commandLine, FileCabinetConsts.ServiceStorageFileShortForm);
        }

        private static bool IsUseLogger(string commandLine)
        {
            return HasCommand(commandLine, FileCabinetConsts.UseLogger);
        }
        
        private static bool IsUseStopWatch(string commandLine)
        {
            return HasCommand(commandLine, FileCabinetConsts.UseStopWatch);
        }

        private static bool HasCommand(string commandLine, string command)
        {
            return commandLine.Contains(command, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}