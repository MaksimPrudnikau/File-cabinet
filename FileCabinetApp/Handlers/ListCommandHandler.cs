using System;

namespace FileCabinetApp.Handlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter _printer;
        
        public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer) : base(service)
        {
            _printer = printer;
        }
        
        public override void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return list of records added to Program.Service
        /// </summary>
        public override void Handle(AppCommandRequest request)
        {
            _printer.Print(Service.GetRecords());
        }
    }
}