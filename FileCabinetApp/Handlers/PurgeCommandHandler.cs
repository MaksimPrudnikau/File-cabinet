using System;

namespace FileCabinetApp.Handlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        /// <summary>
        /// Remove all records marked as deleted from source database
        /// </summary>
        /// <param name="request"></param>
        public override void Handle(AppCommandRequest request)
        {
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