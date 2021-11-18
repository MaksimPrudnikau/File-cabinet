using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.FileSystemService;

namespace FileCabinetApp.Printers.Table
{
    public class TablePrinter : IRecordPrinter
    {
        private IEnumerable<SearchValue.SearchProperty> Properties;
        private IEnumerable<SearchValue> Where;
        private LogicalOperand Operand;

        public TablePrinter()
        {
            
        }

        public TablePrinter(ICollection<SearchValue.SearchProperty> properties, ICollection<SearchValue> where, LogicalOperand operand = LogicalOperand.Or)
        {
            if (properties is null || properties.Count == 0)
            {
                Properties = Enum.GetValues<SearchValue.SearchProperty>();
            }
            else
            {
                Properties = properties;
            }

            Where = where ?? new List<SearchValue>();
            Operand = operand;
        }


        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            var table = new RecordsTable(Properties);

            var correct = true;
            foreach (var record in records)
            {
                switch (Operand)
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

        private bool SatisfyAll(FileCabinetRecord record)
        {
            foreach (var item in Where)
            {
                var value = RecordHelper.GetByAttribute(record, item.Property);
                if (!SearchValue.Equals(new SearchValue($"{item.Property}", value), item))
                {
                    return false;
                }
            }

            return true;
        }
        
        private bool SatisfyAny(FileCabinetRecord record)
        {
            foreach (var item in Where)
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