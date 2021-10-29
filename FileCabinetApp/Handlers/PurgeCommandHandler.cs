using System;

namespace FileCabinetApp.Handlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Purge;
        
        public PurgeCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        /// <summary>
        /// Remove all records marked as deleted from source database
        /// </summary>
        /// <param name="request"></param>
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
            
            try
            { 
                Service.Purge();
            }
            catch (ArgumentNullException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}