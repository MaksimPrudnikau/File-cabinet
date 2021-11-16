using System;
using System.Collections.Generic;
using FileCabinetApp.Export;

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

        public override Statistic GetStat()
        {
            var ticks = TicksMeter.GetElapsedTicks(base.GetStat, out var stat);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(GetStat), ticks);
            return stat;
        }

        public override IEnumerable<FileCabinetRecord> GetRecords()
        {
            var ticks = TicksMeter.GetElapsedTicks(base.GetRecords, out var records);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(GetRecords), ticks);
            return records;
        }

        public override void Restore(FileCabinetServiceSnapshot snapshot)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.Restore, snapshot);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(Restore), ticks);
        }

        public override IEnumerable<int> Delete(SearchValue searchValue)
        {
            var ticks = TicksMeter.GetElapsedTicks(base.Delete, searchValue, out var records);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(Delete), ticks);
            return records;
        }

        public override void Purge()
        {
            var ticks = TicksMeter.GetElapsedTicks(base.Purge);
            Console.WriteLine(EnglishSource.method_execution_duration_ticks, nameof(Purge), ticks);
        }
    }
}