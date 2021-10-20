using System;

namespace FileCabinetApp.Handlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        public EditCommandHandler(IFileCabinetService service) : base(service)
        {
            
        }

        /// <summary>
        /// Edit the record with entered id
        /// </summary>
        /// <param name="request">Object contains command and it's parameters</param>
        /// <exception cref="ArgumentNullException">Request in null</exception>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            if (!int.TryParse(request.Parameters, out var id))
            {
                Console.Error.WriteLine(EnglishSource.id_is_not_an_integer);
                return;
            }

            try
            { 
                var inputParameters = Service.ReadParameters(id);
                
                Service.EditRecord(inputParameters);
                
                Console.WriteLine(EnglishSource.update, id);
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}