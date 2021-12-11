using System;

namespace FileCabinetApp.Handlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        public override RequestCommand Command => RequestCommand.Exit;
        private readonly Action<bool> _runner;

        public ExitCommandHandler(Action<bool> appRunner)
        {
            _runner = appRunner;
        }

        /// <summary>
        /// Exit from application
        /// </summary>
        /// <param name="request">Object contains command and it's parameters</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            Console.WriteLine(EnglishSource.exit);
            _runner(false);
        }
    }
}