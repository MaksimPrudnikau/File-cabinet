using System;

namespace FileCabinetApp.Handlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        public CreateCommandHandler(IFileCabinetService service) : base(service)
        {
        }
        
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
                var parameter = Service.ReadParameters();
                
                Console.WriteLine(EnglishSource.create, Service.CreateRecord(parameter));
            }
            catch (Exception exception) when(exception is ArgumentException or ArgumentNullException)
            {
                Console.Error.WriteLine(exception.Message);
                Create(parameters);
            }
        }
    }
}