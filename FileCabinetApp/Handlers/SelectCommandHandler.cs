using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;
using FileCabinetApp.Printers;
using FileCabinetApp.Printers.Table;

namespace FileCabinetApp.Handlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Select;
        private IRecordPrinter _printer;
        private static readonly LogicalOperand[] Delimiters = {LogicalOperand.And, LogicalOperand.Or};
        
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
            if (string.IsNullOrEmpty(parameters))
            {
                _printer = new TablePrinter();
                return;
            }
            
            const string keyword = "where";
            var split = parameters.Split(keyword);
            var keys = DefaultLineExtractor.GetWords(split[0]);
            var delimiterIndex = FindDelimiter(parameters, Delimiters);

            if (split.Length == 1)
            {
                _printer = new TablePrinter(GetProperties(keys), null, Delimiters[delimiterIndex]);
                return;
            }

            var values = DefaultLineExtractor.ExtractSearchValues(split[1], $"{Delimiters[delimiterIndex]}");
            _printer = new TablePrinter(GetProperties(keys), values, Delimiters[delimiterIndex]);
        }

        private static ICollection<SearchValue.SearchProperty> GetProperties(IEnumerable<string> keys)
        {
            var names = new List<SearchValue.SearchProperty>();
            foreach (var item in keys)
            {
                names.Add(Enum.Parse<SearchValue.SearchProperty>(item, true));
            }

            return names;
        }

        private static int FindDelimiter(string source, IReadOnlyCollection<LogicalOperand> delimiters)
        {
            var findIndex = 0;
            foreach (var delimiter in delimiters)
            {
                if (source.Contains($"{delimiter}", StringComparison.InvariantCultureIgnoreCase))
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