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

            IList<SearchValue> values;
            if (parameters.Contains("and", StringComparison.InvariantCultureIgnoreCase))
            {
                values = Extractor.ExtractSearchValues(split[1], "and");
            }
            else if (parameters.Contains("or", StringComparison.InvariantCultureIgnoreCase))
            {
                values = Extractor.ExtractSearchValues(split[1], "or");
            }
            else if (split[1].Length == 1)
            {
                values = Extractor.ExtractSearchValues(split[1]);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(parameters), EnglishSource.Cannot_resolve_with_input_logical_operand);
            }

            _printer = new TablePrinter {Properties = names, Where = values};
        }
    }
}