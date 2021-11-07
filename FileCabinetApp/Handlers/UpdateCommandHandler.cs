using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;

namespace FileCabinetApp.Handlers.Update
{
    public class UpdateCommandHandler: ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Update;
        private const string ValuesKeyword = "set";
        private const string AttributeKeyword = "where";

        public UpdateCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            if (request.Command != Command)
            {
                NextHandler.Handle(request);
                return;
            }

            var searchValues = GetSearchValues(request.Parameters);
            var where = GetWhereSearchValues(request.Parameters);
        }

        private static IEnumerable<SearchValue> GetWhereSearchValues(string parameters)
        {
            int keywordIndex = parameters.IndexOf(AttributeKeyword, StringComparison.InvariantCultureIgnoreCase);
            parameters = parameters[(keywordIndex + AttributeKeyword.Length + 1)..];
            return Extractor.ExtractSearchValues(parameters, "and");
        }

        private static IEnumerable<SearchValue> GetSearchValues(string parameters)
        {
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

            return Extractor.ExtractSearchValues(parameters[(valuesIndex + ValuesKeyword.Length + 1)..attributeIndex]);
        }
    }
}