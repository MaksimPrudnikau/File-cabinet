using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private static IRecordValidator _validator;
        
        public FileCabinetFilesystemService(IRecordValidator validator)
        {
            _validator = validator;
        }

        public int CreateRecord(Parameter parameters)
        {
            throw new System.NotImplementedException();
        }

        public void EditRecord(Parameter parameters)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new System.NotImplementedException();
        }

        public int GetStat()
        {
            throw new System.NotImplementedException();
        }

        public Parameter ReadParameters(int id = -1)
        {
            throw new System.NotImplementedException();
        }

        public void PrintRecords()
        {
            throw new System.NotImplementedException();
        }
    }
}