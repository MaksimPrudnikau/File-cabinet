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
        public override RequestCommand Command => RequestCommand.Select;
        private IRecordPrinter _printer;
        private static readonly LogicalOperand[] Delimiters = {LogicalOperand.And, LogicalOperand.Or};
        
        public SelectCommandHandler(IFileCabinetService service, IRecordPrinter printer) : base(service)
        {
            _printer = printer;
        }

        /// <summary>
        /// Prints all suitable records by using current printer. By default printer uses <see cref="TablePrinter"/>.
        /// The search is case-sensitive
        /// </summary>
        /// <param name="request">Default command request</param>
        /// <exception cref="ArgumentNullException">Request is null</exception>
        /// <example>
        /// select ----> prints parameters and all records
        /// select firstname, lastname ----> prints only first names and last names
        /// select firstname, lastname where id = '1' ----> prints only first names and last names from records where id equals '1'
        /// select where firstname = 'Ivan' ----> prints all records where firstname equals 'Ivan'
        /// select where rank = 'A' and/or lastname = 'Ivanov' ----> prints all properties from records where rank equals 'A' AND/OR last name equals 'Ivan'
        /// </example>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (_printer is TablePrinter)
            {
                SetTablePrinterProperties(request.Parameters);
            }

            _printer.Print(Service.GetRecords());
        }

        /// <summary>
        /// Create table printer by parsing parameters
        /// </summary>
        /// <param name="parameters">Command line parameters</param>
        private void SetTablePrinterProperties(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                _printer = new TablePrinter();
                return;
            }

            const int headerNamesIndex = 0; 
            const int whereValuesIndex = 1;
            const string keyword = "where";
            
            var split = parameters.TrimStart(' ').TrimEnd(' ').Split(keyword);
            var properties = DefaultLineExtractor.GetWords(split[headerNamesIndex]);
            var delimiterIndex = FindDelimiter(parameters, Delimiters);
            IList<SearchValue> wheres = new List<SearchValue>();

            if (split.Length > 1)
            {
                wheres = DefaultLineExtractor.ExtractSearchValues(split[whereValuesIndex], $"{Delimiters[delimiterIndex]}");
            }
            
            _printer = new TablePrinter(GetProperties(properties), wheres, Delimiters[delimiterIndex]);
        }

        /// <summary>
        /// Cast array of string properties to array of <see cref="SearchValue.SearchProperty"/>
        /// </summary>
        /// <param name="keys">Array of string representation of properties</param>
        /// <returns>Source array parsed to <see cref="SearchValue.SearchProperty"/></returns>
        private static ICollection<SearchValue.SearchProperty> GetProperties(IEnumerable<string> keys)
        {
            var names = new List<SearchValue.SearchProperty>();
            foreach (var item in keys)
            {
                names.Add(Enum.Parse<SearchValue.SearchProperty>(item, true));
            }

            return names;
        }

        /// <summary>
        /// Determine whether the source string contains any from delimiters
        /// </summary>
        /// <param name="source">source string</param>
        /// <param name="delimiters">Array of delimiters to search</param>
        /// <returns>Index of find delimiter</returns>
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