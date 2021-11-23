using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Help;
        private const int CommandHelpIndex = 0;
        private const int ExplanationHelpIndex = 2;
        
        private static readonly string[][] HelpMessages = {
            new[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };
        
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
            
            if (request.Command != Command)
            {
                NextHandler.Handle(request);
                return;
            }
            
            var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length,
                i => string.Equals(i[CommandHelpIndex], request.Parameters, StringComparison.OrdinalIgnoreCase));
            
            Console.Error.WriteLine(index >= 0
                ? HelpMessages[index][ExplanationHelpIndex]
                : $"There is no explanation for '{request.Parameters}' command.");
        }
    }
}