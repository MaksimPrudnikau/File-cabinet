using System.Collections.Generic;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    
    {
        public int CreateRecord(Parameter parameters);
        
        public void EditRecord(Parameter parameters);

        public int GetStat();

        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        public Parameter ReadParameters(int id = -1);
        
        public IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue);
        
        public IEnumerable<FileCabinetRecord> FindByLastName(string searchValue);
        
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue);
    }
}