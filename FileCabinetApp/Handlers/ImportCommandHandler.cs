using System;
using System.IO;

namespace FileCabinetApp.Handlers
{
    public class ImportCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService _service;

        public ImportCommandHandler(IFileCabinetService service)
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
        
        private void Import(string parameters)
        {
            var parametersSplited = parameters.Split(' ');

            if (parametersSplited.Length != 2)
            {
                Console.Error.WriteLine("Wrong data format");
                return;
            }

            var exportFormat = parametersSplited[0];
            var directory = parametersSplited[1];

            if (!File.Exists(directory))
            {
                Console.Error.WriteLine($"File {directory} is not exist");
            }

            var snapshot = new FileCabinetServiceSnapshot();

            using var file = new StreamReader(File.OpenRead(directory));

            try
            {
                switch (exportFormat)
                {
                    case "csv":
                        snapshot.LoadFromCsv(file);
                        break;
                    case "xml":
                        snapshot.LoadFromXml(file);
                        break;
                }

                _service.Restore(snapshot);
            }
            catch (InvalidOperationException exception)
            {
                Console.Error.WriteLine(
                    $"Error: cant deserialize {directory}! {exception.Message}: {exception.InnerException?.Message}");
            }
        }
    }
}