using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class ServiceLogger : FileCabinetServiceDecorator
    {
        private readonly FileLogger _logger;

        public ServiceLogger(IFileCabinetService service, string path) : base(service)
        {
            _logger = new FileLogger(path);
        }

        public override int CreateRecord(FileCabinetRecord record)
        {
            return _logger.LogMethod(base.CreateRecord, record);
        }
        
        public override FileCabinetRecord ReadParameters(int id = -1)
        {
            return _logger.LogMethod(base.ReadParameters, id);
        }

        public override int EditRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            return _logger.LogMethod(base.EditRecord, record);
        }

        public override Statistic GetStat()
        {
            return _logger.LogMethod(base.GetStat);
        }

        public override IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return _logger.LogMethod(base.GetRecords);
        }

        public override IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            return _logger.LogMethod(base.FindByFirstName, searchValue);
        }

        public override IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            return _logger.LogMethod(base.FindByLastName, searchValue);
        }

        public override IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            return _logger.LogMethod(base.FindByDateOfBirth, searchValue);
        }

        public override void Restore(FileCabinetServiceSnapshot snapshot)
        {
            _logger.LogMethod(base.Restore, snapshot);
        }

        public override void Remove(int id)
        {
            _logger.LogMethod(base.Remove, id);
        }

        public override void Purge()
        {
            _logger.LogMethod(base.Purge);
        }
    }
}