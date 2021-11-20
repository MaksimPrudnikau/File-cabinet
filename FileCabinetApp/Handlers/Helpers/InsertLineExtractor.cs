using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers.Helpers
{
    public static class InsertLineExtractor
    {
        /// <summary>
        /// Extract search values from source parameters
        /// </summary>
        /// <param name="parameters">Source string to extract from</param>
        /// <returns>An array of <see cref="SearchValue"/></returns>
        /// <exception cref="ArgumentNullException">Source string is null</exception>
        /// <exception cref="ArgumentException">Cannot find values keyword</exception>
        /// <exception cref="ArgumentException">Number of keys doesnt correspond to number of values</exception>
        public static ICollection<SearchValue> ExtractValues(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            const string keyword = "values";
            if (!parameters.Contains(keyword, StringComparison.InvariantCulture))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                    EnglishSource.Cannot_find_keyword, keyword));
            }

            var split = parameters.Split(keyword);
            var keys = GetKeys(split[0]);
            var values = GetKeys(split[1]);

            if (keys.Count != values.Count)
            {
                throw new ArgumentException(EnglishSource.Number_of_keys_doesnt_correspond_to_number_of_values);
            }

            var created = TryCreateInsertValues(keys, values, out var insertValues);
            return created 
                ? insertValues 
                : null;
        }
        
        private static bool TryCreateInsertValues(IList<string> keys, IList<string> values, out ICollection<SearchValue> insertValues)
        {
            try
            {
                insertValues = new List<SearchValue>(CreateInsertValues(keys, values));
                return true;
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                insertValues = null;
                return false;
            }
        }

        /// <summary>
        /// Create <see cref="SearchValue"/> from source keys and values. The number of them must be equal
        /// </summary>
        /// <param name="keys">Source keys representing record properties</param>
        /// <param name="values">The values that the properties will be filled with</param>
        /// <returns>An array of <see cref="SearchValue"/></returns>
        private static IEnumerable<SearchValue> CreateInsertValues(IList<string> keys, IList<string> values)
        {
            for (var i = 0; i < keys.Count; i++)
            {
                yield return new SearchValue(keys[i], values[i]);
            }
        }

        /// <summary>
        /// Get keys from source string
        /// </summary>
        /// <param name="source"></param>
        /// <returns>An array of keys</returns>
        /// <exception cref="ArgumentException">Parameter values should be enclosed in brackets</exception>
        private static IList<string> GetKeys(string source)
        {
            source = source.TrimStart(' ').TrimEnd(' ');
            if (source[0] != '(' || source[^1] != ')')
            {
                throw new ArgumentException(EnglishSource.Parameter_values_should_be_enclosed_in_brackets);
            }
            
            return DefaultLineExtractor.GetWords(source[1..^1]);
        }
    }
}