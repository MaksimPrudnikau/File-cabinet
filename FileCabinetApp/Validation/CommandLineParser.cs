using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.FileSystemService;
using FileCabinetApp.FileCabinetService.MemoryService;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Validation
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

        /// <summary>
        /// Determine whether the command line contains custom validation rules argument
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>True when command line contains the custom validation rules argument either in full or short form</returns>
        private static bool IsCustomService(string commandLine)
        {
            return HasCommand(commandLine, FileCabinetConsts.CustomValidationRuleFullForm)
                   || HasCommand(commandLine, FileCabinetConsts.CustomValidationRuleShortForm);
        }

        /// <summary>
        /// Determine whether the command line contains filesystem service argument
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>True when command line contains the filesystem service argument either in full or short form</returns>
        private static bool IsFileSystemService(string commandLine)
        {
            return HasCommand(commandLine, FileCabinetConsts.ServiceStorageFileFullForm)
                   || HasCommand(commandLine, FileCabinetConsts.ServiceStorageFileShortForm);
        }

        /// <summary>
        /// Determine whether the command line contains logger argument
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>True when command line contains the logger argument</returns>
        private static bool IsUseLogger(string commandLine)
        {
            return HasCommand(commandLine, FileCabinetConsts.UseLogger);
        }
        
        /// <summary>
        /// Determine whether the command line contains stopwatch argument
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>True when command line contains the stopwatch argument</returns>
        private static bool IsUseStopWatch(string commandLine)
        {
            return HasCommand(commandLine, FileCabinetConsts.UseStopWatch);
        }

        /// <summary>
        /// Determine whether the command line contains source command
        /// </summary>
        /// <param name="commandLine">Command line</param>
        /// <param name="command">Source command</param>
        /// <returns>True when the command line contains source command</returns>
        private static bool HasCommand(string commandLine, string command)
        {
            return commandLine.Contains(command, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}