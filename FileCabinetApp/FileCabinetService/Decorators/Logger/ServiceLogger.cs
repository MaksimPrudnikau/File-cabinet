using System;
using System.Collections.Generic;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService.Iterators;

namespace FileCabinetApp.FileCabinetService.Decorators.Logger
{
    public sealed class ServiceLogger : FileCabinetServiceDecorator, IDisposable
    {
        private readonly FileLogger _logger;
        private bool _disposed;

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

        public override IEnumerable<FileCabinetRecord> GetRecords()
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

        public override IEnumerable<int> Delete(SearchValue attribute, string value)
        {
            return _logger.LogMethod(base.Delete, attribute, value);
        }

        public override void Purge()
        {
            _logger.LogMethod(base.Purge);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(!_disposed)
            { 
                if(disposing)
                {
                    _logger.Dispose();
                }
                
                _disposed = true;
            }
        }
    }
}