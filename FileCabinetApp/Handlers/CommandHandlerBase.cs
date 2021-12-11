
using System;

namespace FileCabinetApp.Handlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        public abstract RequestCommand Command { get; }
        
        public abstract void Handle(AppCommandRequest request);

        public bool CanHandle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Command == Command;
        }
    }
}