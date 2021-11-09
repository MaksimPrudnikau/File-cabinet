using System.Collections.Generic;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService.Iterators;

namespace FileCabinetApp.FileCabinetService
{
    public interface IFileCabinetService
    {
        public int CreateRecord(FileCabinetRecord record);

        public FileCabinetRecord ReadParameters(int id = -1);
        
        public Statistic GetStat();

        public IEnumerable<FileCabinetRecord> GetRecords();

        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue);
        
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue);
        
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue);

        public void Restore(FileCabinetServiceSnapshot snapshot);

        public IEnumerable<int> Delete(SearchValue searchValue);

        public void Purge();

        public void Insert(FileCabinetRecord record);

        public IReadOnlyCollection<int> Update(IEnumerable<SearchValue> values, IList<SearchValue> where);
    }
}