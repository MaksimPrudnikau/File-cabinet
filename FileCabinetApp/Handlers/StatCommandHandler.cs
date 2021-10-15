using System;

namespace FileCabinetApp.Handlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService _service;

        public StatCommandHandler(IFileCabinetService service)
        {
            _service = service;
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
            var stat = _service.GetStat();
            Console.WriteLine(EnglishSource.stat, stat.Count, stat.Deleted);
        }
    }
}