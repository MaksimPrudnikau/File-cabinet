namespace FileCabinetApp.Handlers
{
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected static IFileCabinetService Service;

        protected ServiceCommandHandlerBase(IFileCabinetService service)
        {
            Service = service;
        }
    }
}