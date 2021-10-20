using System;

namespace FileCabinetApp.Handlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Exit from application
        /// </summary>
        /// <param name="request">Object contains command and it's parameters</param>
        public override void Handle(AppCommandRequest request)
        {
            Console.WriteLine(EnglishSource.exit);
            Program.IsRunning = false;
        }
    }
}