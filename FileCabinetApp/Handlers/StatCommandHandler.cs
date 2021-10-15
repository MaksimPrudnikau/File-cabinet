using System;

namespace FileCabinetApp.Handlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        public StatCommandHandler(IFileCabinetService service) : base(service)
        {
            Service = service;
        }
        
        public override void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void Handle(AppCommandRequest request)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Prints the amount of records
        /// </summary>
        private void Stat(string parameters)
        {
            var stat = Service.GetStat();
            Console.WriteLine(EnglishSource.stat, stat.Count, stat.Deleted);
        }
    }
}