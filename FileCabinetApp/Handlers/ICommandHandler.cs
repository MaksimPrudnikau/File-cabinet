namespace FileCabinetApp.Handlers
{
    public interface ICommandHandler
    {
        public void Handle(AppCommandRequest request);
    }
}