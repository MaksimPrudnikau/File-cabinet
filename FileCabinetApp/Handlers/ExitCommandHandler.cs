using System;

namespace FileCabinetApp.Handlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        public override void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void Handle(AppCommandRequest request)
        {
            throw new NotImplementedException();
        }
        
        private void Exit(string parameters)
        {
            Console.WriteLine(EnglishSource.exit);
            Program.IsRunning = false;
        }
    }
}