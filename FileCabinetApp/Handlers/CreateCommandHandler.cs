using System;

namespace FileCabinetApp.Handlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        public CreateCommandHandler(IFileCabinetService service) : base(service)
        {
        }
        

        /// <summary>
        /// Create record in current service
        /// </summary>
        /// <param name="request">Object contains command and it's parameters</param>
        /// <exception cref="ArgumentNullException">Request in null</exception>
        public override void Handle(AppCommandRequest request)
        {
            try
            {
                var parameter = Service.ReadParameters();
                
                Console.WriteLine(EnglishSource.create, Service.CreateRecord(parameter));
            }
            catch (Exception exception) when(exception is ArgumentException or ArgumentNullException)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}