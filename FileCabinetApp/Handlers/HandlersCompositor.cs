using System;
using System.Collections.Generic;

namespace FileCabinetApp.Handlers
{
    public class HandlersCompositor : ICommandHandler
    {
        private readonly List<CommandHandlerBase> _handlers = new();
        
        public void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(request))
                {
                    handler.Handle(request);
                    return;
                }
            }
        }

        public void Add(CommandHandlerBase handler)
        {
            _handlers.Add(handler);
        }
    }
}