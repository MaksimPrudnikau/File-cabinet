using System;

namespace FileCabinetApp.Handlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        public StatCommandHandler(IFileCabinetService service) : base(service)
        {
            Service = service;
        }

        /// <summary>
        /// Prints the amount of records
        /// </summary>
        public override void Handle(AppCommandRequest request)
        {
            var stat = Service.GetStat();
            Console.WriteLine(EnglishSource.stat, stat.Count, stat.Deleted);
        }
    }
}