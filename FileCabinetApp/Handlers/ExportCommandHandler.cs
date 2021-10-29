using System;
using System.IO;

namespace FileCabinetApp.Handlers
{
    public class ExportCommandHandler : CommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Export;
        
        /// <summary>
        /// Serialize all records in file with entered format
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
            var directory =  parametersSplited[1];
            
            if (File.Exists(directory))
            {
                Console.Error.WriteLine(EnglishSource.Export_File_is_exist, directory);
                
                switch (Console.ReadLine())
                {
                    case "Y":
                        break;
                    case "n":
                        Console.WriteLine();
                        return;
                    default:
                        Console.Error.WriteLine("Answer must be either Y or n");
                        return;
                }
            }

            using (var file = new StreamWriter(directory))
            {
                if (!File.Exists(directory))
                {
                    Console.Error.WriteLine($"Export failed: can`t open file {directory}");
                }
                
                var snapshot = FileCabinetMemoryService.MakeSnapshot();

                switch (exportFormat)
                {
                    case "csv":
                        snapshot.SaveToCsv(file);
                        break;
                    case "xml":
                        snapshot.SaveToXml(file);
                        break;
                }
                
                file.Close();
            }
            
            Console.WriteLine(EnglishSource.All_records_are_exported_to_file, directory);
        }
    }
}