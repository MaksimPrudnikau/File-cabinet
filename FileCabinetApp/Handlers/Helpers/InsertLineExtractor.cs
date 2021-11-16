using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers.Helpers
{
    public static class InsertLineExtractor
    {
        public static SearchValue[] ExtractValues(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            const string keyword = "values";
            if (!parameters.Contains(keyword, StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"Cannot find key word '{keyword}'");
            }

            var split = parameters.Split(keyword);
            var keys = GetWords(split[0]);
            var values = GetWords(split[1]);

            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Number of keys doesnt correspond to number of values");
            }

            var created = TryCreateInsertValues(keys, values, out var insertValues);
            return created 
                ? insertValues 
                : null;
        }

        private static bool TryCreateInsertValues(IReadOnlyList<string> keys, IReadOnlyList<string> values, out SearchValue[] insertValues)
        {
            try
            {
                insertValues = CreateInsertValues(keys, values);
                return true;
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                insertValues = null;
                return false;
            }
        }

        private static SearchValue[] CreateInsertValues(IReadOnlyList<string> keys, IReadOnlyList<string> values)
        {
            var insertValues = new List<SearchValue>();
            for (var i = 0; i < keys.Count; i++)
            {
                insertValues.Add(new SearchValue(keys[i], values[i]));
            }

            return insertValues.ToArray();
        }

        private static string[] GetWords(string source)
        {
            if (source[0] != '(' || source[^1] != ')')
            {
                throw new ArgumentException("Parameter values should be enclosed in brackets");
            }
            
            return Extractor.GetWords(source[1..^1]);
        }
    }
}