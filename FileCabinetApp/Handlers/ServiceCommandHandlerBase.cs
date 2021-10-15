namespace FileCabinetApp.Handlers
{
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected static IFileCabinetService Service;

        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            Service = service;
        }
    }
}