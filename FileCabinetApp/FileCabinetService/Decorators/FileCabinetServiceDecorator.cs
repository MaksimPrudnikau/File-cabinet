using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetServiceDecorator : IFileCabinetService
    {
        protected readonly IFileCabinetService Service;

        protected FileCabinetServiceDecorator(IFileCabinetService service)
        {
            Service = service;
        }

        public virtual int CreateRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            Service.CreateRecord(record);
            return record.Id;
        }

        public virtual FileCabinetRecord ReadParameters(int id = -1)
        {
            return Service.ReadParameters(id);
        }

        public virtual int EditRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            record.Id--;
            Service.EditRecord(record);
            return record.Id;
        }

        public virtual Statistic GetStat()
        {
            return Service.GetStat();
        }

        public virtual IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return Service.GetRecords();
        }

        public virtual IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            return Service.FindByFirstName(searchValue);
        }

        public virtual IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            return Service.FindByLastName(searchValue);
        }

        public virtual IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            return Service.FindByDateOfBirth(searchValue);
        }

        public virtual void Restore(FileCabinetServiceSnapshot snapshot)
        {
            Service.Restore(snapshot);
        }

        public virtual void Remove(int id)
        {
            Service.Remove(id);
        }

        public virtual void Purge()
        {
            Service.Purge();
        }
    }
}