namespace FileCabinetApp.Handlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler _nextHandler;

        public abstract void SetNext(ICommandHandler handler);

        public abstract void Handle(AppCommandRequest request);
    }
}