using System.Collections.Generic;
using FileCabinetApp.Export;

namespace FileCabinetApp.FileCabinetService
{
    public interface IFileCabinetService
    {
        public int CreateRecord(FileCabinetRecord record);

        public FileCabinetRecord ReadParameters(int id = -1);
        
        public Statistic GetStat();

        public IEnumerable<FileCabinetRecord> GetRecords();

        public void Restore(FileCabinetServiceSnapshot snapshot);

        public IEnumerable<int> Delete(SearchValue searchValue);

        public void Purge();

        public void Insert(FileCabinetRecord record);

        public IEnumerable<int> Update(IEnumerable<SearchValue> values, IList<SearchValue> where);
    }
}