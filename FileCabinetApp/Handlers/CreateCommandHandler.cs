using System;

namespace FileCabinetApp.Handlers
{
    public class CreateCommandHandler : CommandHandlerBase
    {
        public override void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void Handle(AppCommandRequest request)
        {
            Create(request.Parameters);
        }
        
        /// <summary>
        /// Create a new record according to data user entered
        /// </summary>
        private void Create(string parameters)
        {
            try
            {
                var parameter = Program.Service.ReadParameters();
                
                Console.WriteLine(EnglishSource.create, Program.Service.CreateRecord(parameter));
            }
            catch (Exception exception) when(exception is ArgumentException or ArgumentNullException)
            {
                Console.Error.WriteLine(exception.Message);
                Create(parameters);
            }
        }
    }
}