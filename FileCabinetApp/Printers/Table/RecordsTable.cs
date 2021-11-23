using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.FileSystemService;

namespace FileCabinetApp.Printers.Table
{
    public class RecordsTable
    {
        private readonly List<Column> _columns;
        private int _totalRows;
        private readonly StringBuilder _separateLine;

        public RecordsTable(IEnumerable<SearchValue.SearchProperty> properties)
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

            _separateLine = new StringBuilder();
        }

        /// <summary>
        /// Add source record to table
        /// </summary>
        /// <param name="record">Source record</param>
        public void Add(FileCabinetRecord record)
        {
            foreach (var column in _columns)
            {
                column.Add(RecordHelper.GetByAttribute(record, column.Header));
            }

            _totalRows++;
        }

        /// <summary>
        /// Represents the table as a string
        /// </summary>
        /// <returns><see cref="string"/> representation of current table</returns>
        public override string ToString()
        {
            var table = new StringBuilder();
            var line = new StringBuilder();
            SetSeparatedLine();
            table.Append(GetHeader());
            
            for (var row = 0; row < _totalRows; ++row)
            {
                line.Append('|');
                foreach (var column in _columns)
                {
                    var alignment = column.Header == SearchValue.SearchProperty.Id 
                        ? Alignment.Right 
                        : Alignment.Centering;

                    line.Append(GetValueInFormat(column.Values[row], column.MaxWidth, alignment));
                }
                
                table.AppendLine(line.ToString());
                line.Clear();
            }

            table.AppendLine(_separateLine.ToString());
            return table.ToString();
        }

        /// <summary>
        /// Create a header consists of the names of columns
        /// </summary>
        /// <returns><see cref="string"/> representation of header</returns>
        private StringBuilder GetHeader()
        {
            var header = new StringBuilder();

            header.AppendLine(_separateLine.ToString());
            var currentPos = header.Length;
            foreach (var column in _columns)
            {
                header.Append(GetValueInFormat($"{column.Header}", column.MaxWidth, Alignment.Centering));
            }

            header.Insert(currentPos, '|');
            header.AppendLine();
            header.AppendLine(_separateLine.ToString());
            return header;
        }

        /// <summary>
        /// Create a cell with a given value, expands it with width and orients by alignment
        /// </summary>
        /// <param name="value">Value to place</param>
        /// <param name="maxWidth">The width of the cell</param>
        /// <param name="alignment">Value alignment</param>
        /// <returns><see cref="string"/> representation of the created cell</returns>
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
        
        /// <summary>
        /// Create a new string with the source string and place the source value to the center
        /// </summary>
        /// <param name="source">Source value to centering</param>
        /// <param name="width">The source width</param>
        /// <returns><see cref="string"/> with the source string and place the source value to the center</returns>
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

        /// <summary>
        /// Set the separated line
        /// </summary>
        private void SetSeparatedLine()
        {
            _separateLine.Append('+');
            foreach (var column in _columns)
            {
                _separateLine.Append(GetSeparatedLine(column.MaxWidth));
            }
        }

        /// <summary>
        /// Creates a section of the separate line with the specified length meaning the length of the intended value
        /// </summary>
        /// <param name="length">The estimated length of the inserted value.
        /// Additionally, the left and right are supplemented with one space</param>
        /// <returns>Return the <see cref="string"/> section of a separate line</returns>
        private static string GetSeparatedLine(int length)
        {
            var line = new StringBuilder();
            line.Append('-', length + 2);
            line.Append('+');
            return line.ToString();
        }
    }
}