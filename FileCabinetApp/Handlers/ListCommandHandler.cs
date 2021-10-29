using System;
using System.Collections.Generic;

namespace FileCabinetApp.Handlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.List;
        private readonly Action<IEnumerable<FileCabinetRecord>> _print;

        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> print) :
            base(service)
        {
            _print = print;
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
            
            _print(Service.GetRecords());
        }
    }
}