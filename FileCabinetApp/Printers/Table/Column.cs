using System;
using System.Collections.ObjectModel;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Printers
{
    public class Column
    {
        public SearchValue.SearchProperty Header { get; }
        
        public int MaxWidth { get; private set; }

        public Collection<string> Values { get; }
        
        public Column(SearchValue.SearchProperty header)
        {
            Header = header;
            MaxWidth = $"{header}".Length;
            Values = new Collection<string>();
        }

        public void Add(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            Values.Add(value);
            if (value.Length > MaxWidth)
            {
                MaxWidth = value.Length;
            }
        }
    }
}