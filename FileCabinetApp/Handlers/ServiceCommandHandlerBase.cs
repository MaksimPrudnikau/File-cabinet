namespace FileCabinetApp.Handlers
{
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected static IFileCabinetService Service { get; set; }

        protected ServiceCommandHandlerBase(IFileCabinetService service)
        {
            Service = service;
        }
    }
}