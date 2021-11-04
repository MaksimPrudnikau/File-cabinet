using System;
using System.Collections.Generic;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService.Iterators;

namespace FileCabinetApp.FileCabinetService.Decorators.Meter
{
    public class ServiceMeter : FileCabinetServiceDecorator
    {
        public ServiceMeter(IFileCabinetService service) : base(service)
        {
        }

        public override int CreateRecord(FileCabinetRecord record)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.CreateRecord, record, out var id);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(CreateRecord), ticks);
            return id;
        }

        public override int EditRecord(FileCabinetRecord record)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.EditRecord, record, out var id);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(EditRecord), ticks);
            return id;
        }

        public override Statistic GetStat()
        {
            var ticks = TicksMeter.GetElapsedTicks(base.GetStat, out var stat);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(GetStat), ticks);
            return stat;
        }

        public override IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var ticks = TicksMeter.GetElapsedTicks(base.GetRecords, out var records);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(GetRecords), ticks);
            return records;
        }

        public override IEnumerable<FileCabinetRecord> FindByFirstName(string searchValue)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.FindByFirstName, searchValue, out var records);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(FindByFirstName), ticks);
            return records;
        }
        
        public override IEnumerable<FileCabinetRecord> FindByLastName(string searchValue)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.FindByLastName, searchValue, out var records);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(FindByLastName), ticks);
            return records;
        }
        
        public override IEnumerable<FileCabinetRecord> FindByDateOfBirth(string searchValue)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.FindByDateOfBirth, searchValue, out var records);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(FindByDateOfBirth), ticks);
            return records;
        }

        public override void Restore(FileCabinetServiceSnapshot snapshot)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.Restore, snapshot);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(Restore), ticks);
        }

        public override void Remove(int id)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.Remove, id);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(Remove), ticks);
        }

        public override void Purge()
        {
            var ticks = TicksMeter.GetElapsedTicks(base.Purge);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(Purge), ticks);
        }
    }
}