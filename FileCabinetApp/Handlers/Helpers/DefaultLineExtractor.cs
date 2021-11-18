using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers.Helpers
{
    public static class DefaultLineExtractor
    {
        public static IList<SearchValue> ExtractSearchValues(string source, string delimiter = ",")
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            source = source.Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase);
            source = source.Replace("'", string.Empty, StringComparison.InvariantCultureIgnoreCase);
            var valuesSplited = source.Split(delimiter);
            var searchValues = new List<SearchValue>();
            foreach (var item in valuesSplited)
            {
                var itemSplited = item.Split('=');
                searchValues.Add(new SearchValue(itemSplited[0], itemSplited[1]));
            }

            return searchValues;
        }
        
        public static string[] GetWords(string source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            source = source.Replace(" ", string.Empty, StringComparison.InvariantCulture);
            var builder = new StringBuilder(source);
            builder.Replace("'", string.Empty);
            return builder.ToString().Split(',');
        }
    }
}