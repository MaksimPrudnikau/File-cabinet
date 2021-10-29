using System.Collections.Generic;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        public int CreateRecord(FileCabinetRecord record);

        public FileCabinetRecord ReadParameters(int id = -1);

        public int EditRecord(FileCabinetRecord record);

        public Statistic GetStat();

        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue);
        
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue);
        
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue);

        public void Restore(FileCabinetServiceSnapshot snapshot);

        public void Remove(int id);

        public void Purge();
    }
}