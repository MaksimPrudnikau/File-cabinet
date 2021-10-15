using System;

namespace FileCabinetApp.Handlers
{
    public class ListCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService _service;

        public ListCommandHandler(IFileCabinetService service)
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
        /// Return list of records added to Program.Service
        /// </summary>
        private void List(string parameters)
        {
            foreach (var item in _service.GetRecords())
            {
                item.Print();
            }
        }
    }
}