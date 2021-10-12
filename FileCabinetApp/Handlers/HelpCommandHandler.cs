using System;

namespace FileCabinetApp.Handlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        public override void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void Handle(AppCommandRequest request)
        {
            throw new NotImplementedException();
        }
        
        private void PrintHelp(string parameters)
        {
            var index = Array.FindIndex(FileCabinetConsts.HelpMessages, 0, FileCabinetConsts.HelpMessages.Length,
                i => string.Equals(i[FileCabinetConsts.CommandHelpIndex ], parameters, StringComparison.OrdinalIgnoreCase));
            
            Console.Error.WriteLine(index >= 0
                ? FileCabinetConsts.HelpMessages[index][FileCabinetConsts.ExplanationHelpIndex]
                : $"There is no explanation for '{parameters}' command.");
        }
        
        private static void PrintMissedCommandInfo(string command)
        {
            Console.Error.WriteLine(EnglishSource.no_command, command);
            Console.WriteLine();
        }
    }
}