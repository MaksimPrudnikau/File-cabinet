using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.FileSystemService;

namespace FileCabinetApp.Printers
{
    public class TablePrinter : IRecordPrinter
    {   
        public IEnumerable<SearchValue.SearchProperty> Properties { get; set; }
        
        public IEnumerable<SearchValue> Where { get; set; }

        public LogicalOperand Operand { get; set; }

        private List<FileCabinetRecord> _correctRecords = new();


        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            var table = new Table(Properties);

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

        private IEnumerable<string> GetValues(FileCabinetRecord record)
        {
            foreach (var property in Properties)
            {
                yield return RecordHelper.GetByAttribute(record, property);
            }
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