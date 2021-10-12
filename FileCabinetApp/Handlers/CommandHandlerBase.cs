namespace FileCabinetApp.Handlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        public void SetNext(ICommandHandler handler)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(AppCommandRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}