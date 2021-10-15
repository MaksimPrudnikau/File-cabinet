using System;

namespace FileCabinetApp.Handlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        public ListCommandHandler(IFileCabinetService service) : base(service)
        {
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
            foreach (var item in Service.GetRecords())
            {
                item.Print();
            }
        }
    }
}