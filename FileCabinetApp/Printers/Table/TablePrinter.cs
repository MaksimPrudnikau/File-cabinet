using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.FileSystemService;

namespace FileCabinetApp.Printers.Table
{
    public class TablePrinter : IRecordPrinter
    {
        private readonly IEnumerable<SearchValue.SearchProperty> _properties ;
        private readonly ICollection<SearchValue> _where;
        private readonly LogicalOperand _operand;

        public TablePrinter()
        {
            _properties = Enum.GetValues<SearchValue.SearchProperty>();
            _where = new List<SearchValue>();
            _operand = LogicalOperand.Or;
        }
        
        public TablePrinter(ICollection<SearchValue.SearchProperty> properties, ICollection<SearchValue> where, LogicalOperand operand):
            this()
        {
            if (properties is not null && properties.Count > 0)
            {
                _properties = properties;
            }

            _where = where ?? new List<SearchValue>();
            _operand = operand;
        }
        
        /// <summary>
        /// Find the suitable records from source array and print them in table. If properties to search wasn't not specified it prints
        /// all the properties of record. If 'where' wasn't not specified it prints all records from source array
        /// </summary>
        /// <param name="records">Source record</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            var table = new RecordsTable(_properties);

            var correct = true;
            foreach (var record in records)
            {
                switch (_operand)
                {
                    case LogicalOperand.And:
                        correct = SatisfyAll(record);
                        break;
                    case LogicalOperand.Or:
                        correct = SatisfyAny(record);
                        break;
                }

                if (correct)
                {
                    table.Add(record);
                }
            }
            
            Console.WriteLine(table.ToString());
        }

        /// <summary>
        /// Determine whether the source record properties are equals to current ones
        /// </summary>
        /// <param name="record"></param>
        /// <returns>False if it's not equals to any</returns>
        private bool SatisfyAll(FileCabinetRecord record)
        {
            foreach (var item in _where)
            {
                var value = RecordHelper.GetByAttribute(record, item.Property);
                if (!SearchValue.Equals(new SearchValue($"{item.Property}", value), item))
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Determine whether the source record properties are equals to any from current ones
        /// </summary>
        /// <param name="record"></param>
        /// <returns>True if it's equals to any</returns>
        private bool SatisfyAny(FileCabinetRecord record)
        {
            if (_where.Count == 0)
            {
                return true;
            }
            
            foreach (var item in _where)
            {
                var value = RecordHelper.GetByAttribute(record, item.Property);
                if (SearchValue.Equals(new SearchValue($"{item.Property}", value), item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}