using System;
using System.Globalization;
using System.IO;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;

namespace FileCabinetApp.Handlers
{
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Export;
        
        public ExportCommandHandler(IFileCabinetService service) : base(service)
        {
        }

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
            
            const int correctParametersLength = 2;
            const int outputTypeIndex = 0;
            const int exportPathIndex = 1;

            var parametersSplited = request.Parameters.Split(' ');

            if (parametersSplited.Length != correctParametersLength)
            {
                Console.Error.WriteLine("Wrong data format");
                return;
            }

            var outputFormatIsCorrect = TryGetExportFormat(parametersSplited[outputTypeIndex], out var exportFormat);
            if (!outputFormatIsCorrect)
            {
                return;
            }
            
            var pathIsCorrect = DirectoryIsAllowed(parametersSplited[exportPathIndex]);
            if (!pathIsCorrect)
            {
                return;
            }

            var directory = parametersSplited[exportPathIndex];
            var exported = TryExport(directory, exportFormat);
            if (exported)
            {
                Console.WriteLine(EnglishSource.All_records_are_exported_to_file, directory);
            }
        }

        private static bool TryExport(string path, ExportFormat? format)
        {
            try
            {
                Export(path, format);
                return true;
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        /// <summary>
        /// Export all records from current service in file with the input path and input format. If file is not exist
        /// it will be created
        /// </summary>
        /// <param name="path">Absolute path to the export file</param>
        /// <param name="format">Format of the export file</param>
        /// <exception cref="ArgumentNullException">Export format is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Export format is not supported</exception>
        private static void Export(string path, ExportFormat? format)
        {
            if (format is null)
            {
                throw new ArgumentNullException(nameof(format), EnglishSource.Export_format_cannot_be_null);
            }
            
            using var file = new StreamWriter(path);
            var snapshot = new FileCabinetServiceSnapshot(Service);
            switch (format)
            {
                case ExportFormat.Csv:
                    snapshot.SaveToCsv(file);
                    break;
                case ExportFormat.Xml:
                    snapshot.SaveToXml(file);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format),
                        string.Format(CultureInfo.CurrentCulture, EnglishSource.Export_type_is_not_supported, format));
            }

            file.Close();
        }

        /// <summary>
        /// Determine whether the directory is allowed, file of source directory is exist and if it is
        /// request permission to rewrite
        /// </summary>
        /// <param name="sourceDirectory">Source directory</param>
        /// <returns>True if file is not exist or rewrite is allowed</returns>
        private static bool DirectoryIsAllowed(string sourceDirectory)
        {
            if (!File.Exists(sourceDirectory))
            {
                return true;
            }

            Console.WriteLine(EnglishSource.Export_File_is_exist);
            return AllowRewriteIfExist();
        }

        /// <summary>
        /// Get the permission to rewrite the file
        /// </summary>
        /// <returns>True of allowed</returns>
        private static bool AllowRewriteIfExist()
        {
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "Y":
                        return true;
                    case "n":
                        Console.WriteLine();
                        return false;
                    default:
                        Console.WriteLine(EnglishSource.Answer_must_be_either_Y_or_n);
                        continue;
                }
            }
        }

        private static bool TryGetExportFormat(string parameters, out ExportFormat? format)
        {
            try
            {
                format = GetExportFormat(parameters);
                return true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                format = null;
                return false;
            }
        }

        /// <summary>
        /// Parse string representation of <see cref="ExportFormat"/>
        /// </summary>
        /// <param name="parameters">Source string to parse</param>
        /// <returns>Parsed <see cref="ExportFormat"/></returns>
        private static ExportFormat GetExportFormat(string parameters)
        {
            return Enum.Parse<ExportFormat>(parameters, true);
        }
    }
}