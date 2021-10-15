using System;
using System.Globalization;

namespace FileCabinetApp.Handlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        public RemoveCommandHandler(IFileCabinetService service) : base(service)
        {
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