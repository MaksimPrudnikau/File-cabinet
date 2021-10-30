using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Stat;
        
        public StatCommandHandler(IFileCabinetService service) : base(service)
        {
            Service = service;
        }

        /// <summary>
        /// Prints the amount of records
        /// </summary>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            if (request.Command != Command)
            {
                NextHandler.Handle(request);
                return;
            }
            
            var stat = Service.GetStat();
            Console.WriteLine(EnglishSource.stat, stat.Count, stat.Deleted);
        }
    }
}