using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Printers;

namespace FileCabinetApp.Handlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.List;
        private readonly IRecordPrinter _printer;

        public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer) :
            base(service)
        {
            _printer = printer;
        }

        /// <summary>
        /// Return list of records added to current service
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
            
            _printer.Print(Service.GetRecords());
        }
    }
}