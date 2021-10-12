using System;

namespace FileCabinetApp.Handlers
{
    public class PurgeCommandHandler : CommandHandlerBase
    {
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
                Program.Service.Purge();
            }
            catch (ArgumentNullException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}