using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        public override RequestCommand Command => RequestCommand.Help;

        /// <summary>
        /// Prints help message
        /// </summary>
        /// <param name="request">Object contains command and it's parameters</param>
        /// <exception cref="ArgumentNullException">Request in null</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
        }
    }
}