using System.Collections.Generic;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService.Iterators;

namespace FileCabinetApp.FileCabinetService
{
    public interface IFileCabinetService
    {
        public int CreateRecord(FileCabinetRecord record);

        public FileCabinetRecord ReadParameters(int id = -1);

        public int EditRecord(FileCabinetRecord record);

        public Statistic GetStat();

        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        public IRecordIterator FindByFirstName(string searchValue);
        
        public IRecordIterator FindByLastName(string searchValue);
        
        public IRecordIterator FindByDateOfBirth(string searchValue);

        public void Restore(FileCabinetServiceSnapshot snapshot);

        public void Remove(int id);

        public void Purge();
    }
}