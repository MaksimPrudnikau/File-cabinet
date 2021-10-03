using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FileCabinetGenerator
{
    public class Options
    {
        public OutputType Type { get; set; }

        public string FileName { get; set; }

        public long Count { get; set; }

        public int StartId { get; set; }

        
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

        private void ParseOutputType(IReadOnlyList<string> args)
        {
            const int fullFormIndex = 0;
            const int shortFormIndex = 1;

            if (IsFullForm(args[fullFormIndex]))
            {
                Type = (OutputType) Enum.Parse(typeof(OutputType), GetLongFormValue(args[fullFormIndex]));
            }

            Type = (OutputType) Enum.Parse(typeof(OutputType), args[shortFormIndex]);
        }

        private void ParseOutputFileName(IReadOnlyList<string> args)
        {
            const int fullFormIndex = 1;
            const int shortFormIndex = 3;
            
            if (IsFullForm(args[fullFormIndex]))
            {
                FileName = GetLongFormValue(args[fullFormIndex]);
            }
            
            FileName = args[shortFormIndex];
        }
        
        private void ParseRecordsAmount(IReadOnlyList<string> args)
        {
            const int fullFormIndex = 2;
            const int shortFormIndex = 5;

            if (args[fullFormIndex].StartsWith("--"))
            {
                Count = Convert.ToInt64(GetLongFormValue(args[fullFormIndex]));
            }
            
            Count = Convert.ToInt64(args[shortFormIndex]);
        }
        
        private void ParseStartId(IReadOnlyList<string> args)
        {
            const int fullFormIndex = 4;
            const int shortFormIndex = 7;

            if (IsFullForm(args[fullFormIndex]))
            {
                StartId = Convert.ToInt32(GetLongFormValue(args[fullFormIndex]));
            }
            
            StartId = Convert.ToInt32(args[shortFormIndex]);
        }

        private bool IsFullForm(string parameter)
        {
            return parameter.StartsWith("--");
        }
        
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