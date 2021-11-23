using System;
using System.Collections.Generic;
using FileCabinetApp.Export;

namespace FileCabinetApp.FileCabinetService.Decorators.Logger
{
    public sealed class ServiceLogger : FileCabinetServiceDecorator, IDisposable
    {
        private readonly FileSystemLogger _systemLogger;
        private bool _disposed;

        public ServiceLogger(IFileCabinetService service, string path) : base(service)
        {
            _systemLogger = new FileSystemLogger(path);
        }

        public override int CreateRecord()
        {
            return _systemLogger.LogMethod(base.CreateRecord);
        }

        public override Statistic GetStat()
        {
            return _systemLogger.LogMethod(base.GetStat);
        }

        public override IEnumerable<FileCabinetRecord> GetRecords()
        {
            return _systemLogger.LogMethod(base.GetRecords);
        }

        public override void Restore(FileCabinetServiceSnapshot snapshot)
        {
            _systemLogger.LogMethod(base.Restore, snapshot);
        }

        public override IEnumerable<int> Delete(SearchValue searchValue)
        {
            return _systemLogger.LogMethod(base.Delete, searchValue);
        }

        public override void Purge()
        {
            _systemLogger.LogMethod(base.Purge);
        }

        public override void Insert(FileCabinetRecord record)
        {
            _systemLogger.LogMethod(base.Insert, record);
        }

        public override IEnumerable<int> Update(IEnumerable<SearchValue> values, IList<SearchValue> @where)
        {
            return _systemLogger.LogMethod(base.Update, values, where);
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
                    _systemLogger.Dispose();
                }
                
                _disposed = true;
            }
        }
    }
}