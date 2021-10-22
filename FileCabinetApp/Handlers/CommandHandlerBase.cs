namespace FileCabinetApp.Handlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        protected ICommandHandler NextHandler { get; set; }

        public virtual void SetNext(ICommandHandler handler)
        {
            NextHandler = handler;
        }

        public abstract void Handle(AppCommandRequest request);
    }
}