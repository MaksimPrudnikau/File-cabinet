using System;
using System.Collections.Generic;
using FileCabinetApp.Export;

namespace FileCabinetApp.FileCabinetService.Decorators
{
    public class FileCabinetServiceDecorator : IFileCabinetService
    {
        private readonly IFileCabinetService _service;

        protected FileCabinetServiceDecorator(IFileCabinetService service)
        {
            _service = service;
        }

        public virtual int CreateRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            _service.CreateRecord(record);
            return record.Id;
        }

        public virtual FileCabinetRecord ReadParameters(int id = -1)
        {
            return _service.ReadParameters(id);
        }

        public virtual int EditRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            record.Id--;
            _service.EditRecord(record);
            return record.Id;
        }

        public virtual Statistic GetStat()
        {
            return _service.GetStat();
        }

        public virtual IEnumerable<FileCabinetRecord> GetRecords()
        {
            return _service.GetRecords();
        }

        public virtual IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            return _service.FindByFirstName(searchValue);
        }

        public virtual IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            return _service.FindByLastName(searchValue);
        }

        public virtual IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            return _service.FindByDateOfBirth(searchValue);
        }

        public virtual void Restore(FileCabinetServiceSnapshot snapshot)
        {
            _service.Restore(snapshot);
        }

        public virtual IEnumerable<int> Delete(SearchValue attribute, string value)
        {
            throw new NotImplementedException();
        }

        public virtual void Purge()
        {
            _service.Purge();
        }

        public void Insert(FileCabinetRecord record)
        {
            throw new NotImplementedException();
        }
    }
}