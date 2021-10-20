namespace FileCabinetApp.Handlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        public ICommandHandler NextHandler;

        public virtual void SetNext(ICommandHandler handler)
        {
            NextHandler = handler;
        }

        public abstract void Handle(AppCommandRequest request);
    }
}