using System;
using System.Collections.Generic;
using FileCabinetApp.Export;

namespace FileCabinetApp.FileCabinetService.Decorators
{
    public abstract class FileCabinetServiceDecorator : IFileCabinetService
    {
        private readonly IFileCabinetService _service;

        protected FileCabinetServiceDecorator(IFileCabinetService service)
        {
            _service = service;
        }

        public virtual int CreateRecord()
        {
            return _service.CreateRecord();
        }

        public virtual Statistic GetStat()
        {
            return _service.GetStat();
        }

        public virtual IEnumerable<FileCabinetRecord> GetRecords()
        {
            return _service.GetRecords();
        }

        public virtual void Restore(FileCabinetServiceSnapshot snapshot)
        {
            _service.Restore(snapshot);
        }

        public virtual IEnumerable<int> Delete(SearchValue searchValue)
        {
            return _service.Delete(searchValue);
        }

        public virtual void Purge()
        {
            _service.Purge();
        }

        public virtual void Insert(FileCabinetRecord record)
        {
            _service.Insert(record);
        }

        public virtual IEnumerable<int> Update(IList<SearchValue> values, IList<SearchValue> where)
        {
            return _service.Update(values, where);
        }
    }
}