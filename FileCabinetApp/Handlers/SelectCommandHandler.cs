using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;
using FileCabinetApp.Printers;

namespace FileCabinetApp.Handlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Select;
        private IRecordPrinter _printer;
        private static readonly string[] Delimiters = {"and", "or"};
        
        public SelectCommandHandler(IFileCabinetService service, IRecordPrinter printer) : base(service)
        {
            _printer = printer;
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

            if (_printer is TablePrinter)
            {
                SetPrinterAsTable(request.Parameters);
            }

            _printer.Print(Service.GetRecords());
        }

        private void SetPrinterAsTable(string parameters)
        {
            const string keyword = "where";
            if (!parameters.Contains(keyword, StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"Cannot find key word '{keyword}'");
            }

            var split = parameters.Split(keyword);
            var keys = Extractor.GetWords(split[0]);

            var names = new List<SearchValue.SearchProperty>();
            foreach (var item in keys)
            {
                names.Add(Enum.Parse<SearchValue.SearchProperty>(item, true));
            }

            var delimiterIndex = FindDelimiter(parameters, Delimiters);

            var values = Extractor.ExtractSearchValues(split[1], Delimiters[delimiterIndex]);

            _printer = new TablePrinter 
            {
                Properties = names,
                Where = values,
                Operand = Enum.Parse<LogicalOperand>(Delimiters[delimiterIndex], true)
            };
            
            _printer.Print(Service.GetRecords());
        }

        private static int FindDelimiter(string source, IReadOnlyCollection<string> delimiters)
        {
            var findIndex = 0;
            foreach (var delimiter in delimiters)
            {
                if (source.Contains(delimiter, StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                findIndex++;
            }

            if (findIndex >= delimiters.Count)
            {
                return 0;
            }
            
            return findIndex;
        }
    }
}