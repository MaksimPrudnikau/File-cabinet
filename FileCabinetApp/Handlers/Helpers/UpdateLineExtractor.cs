using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers.Helpers
{
    public static class UpdateLineExtractor
    {
        private const string ValuesKeyword = "set";
        private const string AttributeKeyword = "where";

        public static IList<SearchValue> GetWhereSearchValues(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            var keywordIndex = parameters.IndexOf(AttributeKeyword, StringComparison.InvariantCultureIgnoreCase);
            parameters = parameters[(keywordIndex + AttributeKeyword.Length + 1)..];
            return DefaultLineExtractor.ExtractSearchValues(parameters, "and");
        }

        public static IEnumerable<SearchValue> GetSearchValues(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            if (!parameters.Contains(ValuesKeyword, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException($"Cant find {ValuesKeyword} keyword");
            }
            
            if (!parameters.Contains(AttributeKeyword, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Cant find {AttributeKeyword} keyword");
            }

            var valuesIndex = parameters.IndexOf(ValuesKeyword, StringComparison.InvariantCultureIgnoreCase);
            var attributeIndex = parameters.IndexOf(AttributeKeyword, StringComparison.InvariantCultureIgnoreCase);
            if (valuesIndex > attributeIndex)
            {
                throw new ArgumentException(
                    $"The location of '{ValuesKeyword}' is arranged previous '{AttributeKeyword}'");
            }

            return DefaultLineExtractor.ExtractSearchValues(parameters[(valuesIndex + ValuesKeyword.Length + 1)..attributeIndex]);
        }
    }
}