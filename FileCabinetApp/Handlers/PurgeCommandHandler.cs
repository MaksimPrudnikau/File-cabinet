using System;

namespace FileCabinetApp.Handlers
{
    public class PurgeCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService _service;

        public PurgeCommandHandler(IFileCabinetService service)
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
        
        private void Purge(string parameters)
        {
            try
            { 
                _service.Purge();
            }
            catch (ArgumentNullException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}