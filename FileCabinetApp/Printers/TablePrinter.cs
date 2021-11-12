using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Printers
{
    public class TablePrinter : IRecordPrinter
    {   
        public IEnumerable<SearchValue.SearchProperty> Properties { get; set; }
        
        public IEnumerable<SearchValue> Where { get; set; }

        public bool LogicalAnd { get; set; }

        private readonly Dictionary<SearchValue.SearchProperty, int> _currentWidths = new();

        private long _width;

        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            FillDictionary();
            var test = new StringBuilder();
            Console.WriteLine(CreateHeader());

            foreach (var record in records)
            {
            }
        }

        private void FillDictionary()
        {
            foreach (var property in Properties)
            {
                if (!_currentWidths.ContainsKey(property))
                {
                    _currentWidths.Add(property, $"{property}".Length);
                }
            }
        }

        private StringBuilder CreateHeader()
        {
            var line = new StringBuilder();
            var values = new List<string>();

            foreach (var property in Properties)
            {
                values.Add($"{property}");
            }

            var names = GetValues(values);
            var separateLine = GetSeparateLine();
            
            line.Append(separateLine);
            line.Append(names);
            line.Append(separateLine);
            return line;
        }

        private StringBuilder GetValues(IEnumerable<string> values)
        {
            var line = new StringBuilder();
            line.Append('|');

            foreach (var record in values)
            {
                line.Append($" {record} |");
            }

            if (line.Length > _width)
            {
                _width = line.Length;
            }

            line.AppendLine();
            return line;
        }

        private StringBuilder GetSeparateLine()
        {
            var line = new StringBuilder();
            line.Append('+');
            foreach (var width in _currentWidths.Values)
            {
                line.Append('-', width + 2);
                line.Append('+');
            }
            
            line.AppendLine();
            return line;
        }
    }
}