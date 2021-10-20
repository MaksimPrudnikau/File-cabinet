using System;
using System.Collections.Generic;

namespace FileCabinetApp.Handlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
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
            _print(Service.GetRecords());
        }
    }
}