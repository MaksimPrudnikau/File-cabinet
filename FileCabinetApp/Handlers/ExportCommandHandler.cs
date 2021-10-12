using System;
using System.IO;

namespace FileCabinetApp.Handlers
{
    public class ExportCommandHandler : CommandHandlerBase
    {
        public override void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void Handle(AppCommandRequest request)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Serialize all records in file with entered format
        /// </summary>
        /// <param name="parameters">Output file format</param>
        private void Export(string parameters)
        {
            var parametersSplited = parameters.Split(' ');
            
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