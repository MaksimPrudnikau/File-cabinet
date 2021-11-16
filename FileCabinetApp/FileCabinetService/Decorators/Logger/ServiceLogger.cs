using System;
using System.Collections.Generic;
using FileCabinetApp.Export;

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

        public override Statistic GetStat()
        {
            return _logger.LogMethod(base.GetStat);
        }

        public override IEnumerable<FileCabinetRecord> GetRecords()
        {
            return _logger.LogMethod(base.GetRecords);
        }

        public override void Restore(FileCabinetServiceSnapshot snapshot)
        {
            _logger.LogMethod(base.Restore, snapshot);
        }

        public override IEnumerable<int> Delete(SearchValue searchValue)
        {
            return _logger.LogMethod(base.Delete, searchValue);
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