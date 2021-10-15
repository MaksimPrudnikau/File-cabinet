using System;
using System.Globalization;

namespace FileCabinetApp.Handlers
{
    public class RemoveCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService _service;

        public RemoveCommandHandler(IFileCabinetService service)
        {
            _service = service;
        }
        
        public override void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void Handle(AppCommandRequest request)
        {
            throw new NotImplementedException();
        }
        
        private void Remove(string parameters)
        {
            try
            {
                var id = Convert.ToInt32(parameters, CultureInfo.InvariantCulture);
                
                _service.Remove(id);

                Console.WriteLine(EnglishSource.Record_is_removed, id);
            }
            catch (Exception exception) when (exception is OverflowException or FormatException or ArgumentException)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}