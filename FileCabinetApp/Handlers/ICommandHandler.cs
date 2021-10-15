namespace FileCabinetApp.Handlers
{
    public interface ICommandHandler
    {
        public void SetNext(ICommandHandler handler);

        public void Handle(AppCommandRequest request);
    }
}