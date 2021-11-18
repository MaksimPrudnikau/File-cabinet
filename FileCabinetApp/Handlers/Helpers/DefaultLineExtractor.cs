using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers.Helpers
{
    public static class DefaultLineExtractor
    {
        /// <summary>
        /// Create <see cref="IList{T}"/> of <see cref="SearchValue"/> from source string with values enumerated by source delimiter
        /// </summary>
        /// <param name="source">source string</param>
        /// <param name="delimiter">source delimiter</param>
        /// <returns><see cref="IList{T}"/> of <see cref="SearchValue"/></returns>
        /// <exception cref="ArgumentNullException">Source string is null or empty</exception>
        public static IList<SearchValue> ExtractSearchValues(string source, string delimiter = ",")
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            source = source.Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase);
            source = source.Replace("'", string.Empty, StringComparison.InvariantCultureIgnoreCase);
            var valuesSplited = source.Split(delimiter);
            var searchValues = new List<SearchValue>();
            foreach (var item in valuesSplited)
            {
                if (!item.Contains("=", StringComparison.Ordinal))
                {
                    throw new ArgumentException("Cannot find equal sign");
                }
                
                var itemSplited = item.Split('=');
                searchValues.Add(new SearchValue(itemSplited[0], itemSplited[1]));
            }

            return searchValues;
        }
        
        public static string[] GetWords(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return Array.Empty<string>();
            }

            source = source.Replace(" ", string.Empty, StringComparison.InvariantCulture);
            var builder = new StringBuilder(source);
            builder.Replace("'", string.Empty);
            return builder.ToString().Split(',');
        }
    }
}