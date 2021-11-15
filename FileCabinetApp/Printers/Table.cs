using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.FileSystemService;

namespace FileCabinetApp.Printers
{
    public class Table
    {
        private readonly List<Column> _columns;
        private int _totalRows;

        public Table(IEnumerable<SearchValue.SearchProperty> properties)
        {
            if (properties is null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            
            _columns = new List<Column>();
            foreach (var property in properties)
            {
                _columns.Add(new Column(property));
            }
        }

        public void Add(FileCabinetRecord record)
        {
            foreach (var column in _columns)
            {
                column.Add(RecordHelper.GetByAttribute(record, column.Header));
            }

            _totalRows++;
        }

        public override string ToString()
        {
            var table = new StringBuilder();
            for (var row = 0; row < _totalRows; ++row)
            {
                foreach (var column in _columns)
                {
                    var alignment = Alignment.Centering;
                    if (column.Header is SearchValue.SearchProperty.Id)
                    {
                        alignment = Alignment.Right;
                    }
                    
                    table.Append(GetValueInFormat( column.Values[row], column.MaxWidth, alignment));
                }

                table.AppendLine();
            }

            return table.ToString();
        }

        private static StringBuilder GetValueInFormat(string value, int maxWidth, Alignment alignment)
        {
            var table = new StringBuilder();
            var formatString = alignment switch
            {
                Alignment.Right => string.Format(CultureInfo.InvariantCulture, " {{0, {0}}} |", maxWidth),
                _ => string.Format(CultureInfo.InvariantCulture, " {{0, -{0}}} |", maxWidth)
            };

            if (alignment is Alignment.Centering)
            {
                value = CenteredString(value, maxWidth);
            }
            
            table.AppendFormat(CultureInfo.InvariantCulture, formatString, value);
            return table;
        }
        
        private static string CenteredString(string source, int width)
        {
            if (source.Length >= width)
            {
                return source;
            }

            var leftPadding = (width - source.Length) / 2;
            var rightPadding = width - source.Length - leftPadding;
            
            return new string(' ', leftPadding) + source + new string(' ', rightPadding);
        }
    }
}