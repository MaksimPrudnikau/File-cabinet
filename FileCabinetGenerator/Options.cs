using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FileCabinetGenerator
{
    public class Options
    {
        public OutputType Type { get; private set; }

        public string FileName { get; private set; }

        public long Count { get; private set; }

        public int StartId { get; private set; }

        
        public Options([NotNull] IReadOnlyList<string> args)
        {
            try
            {
                ParseOutputType(args);

                ParseOutputFileName(args);
            
                ParseRecordsAmount(args);
            
                ParseStartId(args);
            }
            catch (ArgumentException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Set output type read from command line
        /// </summary>
        /// <param name="args">command line split into words</param>
        private void ParseOutputType(IReadOnlyList<string> args)
        {
            const int fullFormIndex = 0;
            const int shortFormIndex = 1;

            Type = (OutputType) Enum.Parse(typeof(OutputType), ParseOption(args, fullFormIndex, shortFormIndex));
        }

        /// <summary>
        /// Set output file name read from command line
        /// </summary>
        /// <param name="args">command line split into words</param>
        private void ParseOutputFileName(IReadOnlyList<string> args)
        {
            const int fullFormIndex = 1;
            const int shortFormIndex = 3;

            FileName = ParseOption(args, fullFormIndex, shortFormIndex);
        }
        
        /// <summary>
        /// Set the total amount of records to create read from command line
        /// </summary>
        /// <param name="args">command line split into words</param>
        private void ParseRecordsAmount(IReadOnlyList<string> args)
        {
            const int fullFormIndex = 2;
            const int shortFormIndex = 5;

            Count = Convert.ToInt64(ParseOption(args, fullFormIndex, shortFormIndex));
        }
        
        /// <summary>
        /// Set the initial record`s id read from command line
        /// </summary>
        /// <param name="args">command line split into words</param>
        private void ParseStartId(IReadOnlyList<string> args)
        {
            const int fullFormIndex = 4;
            const int shortFormIndex = 7;

            StartId = Convert.ToInt32(ParseOption(args, fullFormIndex, shortFormIndex));
        }

        /// <summary>
        /// Get the option value from command line 
        /// </summary>
        /// <param name="args">Command line split into words</param>
        /// <param name="fullFormIndex">The index of option when all the commands are full-form</param>
        /// <param name="shortFormIndex">The index of option when all the commands are short-form</param>
        /// <returns>Read value</returns>
        private string ParseOption(IReadOnlyList<string> args, int fullFormIndex, int shortFormIndex)
        {
            return IsFullFormOption(args[fullFormIndex]) 
                ? GetLongFormValue(args[fullFormIndex]) 
                : args[shortFormIndex];
        }

        /// <summary>
        /// Determine if the argument is a full-form option
        /// </summary>
        /// <param name="parameter">Source argument</param>
        /// <returns>Whether the source parameter starts with '--'</returns>
        private bool IsFullFormOption(string parameter)
        {
            return parameter.StartsWith("--");
        }
        
        /// <summary>
        /// Extract value from full-form option
        /// </summary>
        /// <param name="parameter">Source option</param>
        /// <returns>The value after '=' symbol</returns>
        /// <exception cref="ArgumentException">Equal symbol is not found</exception>
        private string GetLongFormValue(string parameter)
        {
            var equalSign = parameter.IndexOf('=');
            if (equalSign == -1)
            {
                throw new ArgumentException($"Cannot find '=' in {parameter}");
            }
            
            return parameter[(equalSign + 1)..];
        }
    }
}