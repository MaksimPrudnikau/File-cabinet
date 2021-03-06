using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Create;

        public CreateCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        /// <summary>
        /// Create record in current service. If default service is selected, parameters from the custom one
        /// are not requested
        /// </summary>
        /// <param name="request">Object contains command and it's parameters</param>
        /// <exception cref="ArgumentNullException">Request in null</exception>
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
                Console.WriteLine(EnglishSource.create, Service.CreateRecord());
            }
            catch (Exception exception) when(exception is ArgumentException or ArgumentNullException)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}