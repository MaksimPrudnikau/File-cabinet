using System.Collections.Generic;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    
    {
        public int CreateRecord(FileCabinetRecord record);
        
        public void EditRecord(FileCabinetRecord record);

        public Statistic GetStat();

        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        public FileCabinetRecord ReadParameters(int id = -1);
        
        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue);
        
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue);
        
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue);

        public void Restore(FileCabinetServiceSnapshot snapshot);

        public void Remove(int id);

        public void Purge();
    }
}