using System;
using System.Globalization;

namespace FileCabinetApp.Handlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Delete;
        
        public RemoveCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        /// <summary>
        /// Remove record from current service. If service storage is file, it marks the record as deleted
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

            try
            {
                var id = Convert.ToInt32(request.Parameters, CultureInfo.InvariantCulture);
                
                Service.Remove(id);

                Console.WriteLine(EnglishSource.Record_is_removed, id);
            }
            catch (Exception exception) when (exception is OverflowException or FormatException or ArgumentException)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}