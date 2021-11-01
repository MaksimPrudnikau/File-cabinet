using System;
using System.Globalization;
using System.IO;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Handlers
{
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Import;

        public ImportCommandHandler(IFileCabinetService service) : base(service)
        {
            
        }

        /// <summary>
        /// Import records from source file
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
            
            var parametersSplited = request.Parameters.Split(' ');

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

            try
            {
                using var file = new StreamReader(new FileStream(directory, FileMode.Open));
                
                switch (exportFormat)
                {
                    case "csv":
                        snapshot.LoadFromCsv(file);
                        break;
                    case "xml":
                        snapshot.LoadFromXml(file);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(request),
                            string.Format(CultureInfo.InvariantCulture,
                                EnglishSource.ImportCommandHandler_The_format_is_not_supported, exportFormat));
                }

                Service.Restore(snapshot);
            }
            catch (InvalidOperationException exception)
            {
                Console.Error.WriteLine(
                    $"Error: cant deserialize {directory}! {exception.Message}: {exception.InnerException?.Message}");
            }
            catch (SystemException exception) when (exception is ArgumentException or IOException)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}