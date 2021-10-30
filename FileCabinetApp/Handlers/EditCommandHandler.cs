using System;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Edit;

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
            
            if (request.Command != Command)
            {
                NextHandler.Handle(request);
                return;
            }
            
            if (!int.TryParse(request.Parameters, out var id))
            {
                Console.Error.WriteLine(EnglishSource.id_is_not_an_integer);
                return;
            }

            try
            {
                var record = Service.ReadParameters();
                Console.WriteLine(EnglishSource.update,  Service.EditRecord(record));
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}