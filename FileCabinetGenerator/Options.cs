
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FileCabinetGenerator
{
    public class Options
    {
        private OutputType Type { get; set; }

        private string FileName { get; set; }

        private long Count { get; set; }

        private long StartId { get; set; }

        
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
            const int outputTypeIndex = 0;
            Type = args[outputTypeIndex].StartsWith("--")
                ? (OutputType) Enum.Parse(typeof(OutputType), GetLongFormValue(args[outputTypeIndex]))
                : (OutputType) Enum.Parse(typeof(OutputType), args[outputTypeIndex + 1].ToLowerInvariant());
        }

        private void ParseOutputFileName(IReadOnlyList<string> args)
        {
            const int nameIndex = 1;
            FileName = args[nameIndex].StartsWith("--") 
                ? GetLongFormValue(args[nameIndex]) 
                : args[nameIndex + 1].ToLowerInvariant();
        }
        
        private void ParseRecordsAmount(IReadOnlyList<string> args)
        {
            const int recordsAmountIndex = 2;
            Count = args[recordsAmountIndex].StartsWith("--")
                ? Convert.ToInt64(GetLongFormValue(args[recordsAmountIndex]))
                : Convert.ToInt64(args[recordsAmountIndex + 1].ToLowerInvariant());
        }
        
        private void ParseStartId(IReadOnlyList<string> args)
        {
            const int recordsAmountIndex = 3;
            StartId = args[recordsAmountIndex].StartsWith("--")
                ? Convert.ToInt64(GetLongFormValue(args[recordsAmountIndex]))
                : Convert.ToInt64(args[recordsAmountIndex + 1].ToLowerInvariant());
        }

        private string GetLongFormValue(string parameter)
        {
            var equalSign = parameter.IndexOf('=');
            if (equalSign == -1)
            {
                throw new ArgumentException($"Cannot find '=' in {parameter}");
            }
            
            return parameter[(equalSign + 1)..].ToLowerInvariant();
        }
    }
}