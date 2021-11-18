using System;
using System.Globalization;
using System.IO;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;

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
                Console.Error.WriteLine(EnglishSource.Wrong_data_format);
                return;
            }

            const int exportFormatIndex = 0;
            const int directoryIndex = 1;

            var outputFormatIsCorrect = TryGetExportFormat(parametersSplited[exportFormatIndex], out var exportFormat);

            var directory = parametersSplited[directoryIndex];
            var directoryIsCorrect = TryGetDirectory(directory);

            if (!outputFormatIsCorrect || !directoryIsCorrect)
            {
                return;
            }

            if (TryImport(directory, exportFormat, out var snapshot))
            {
                Service.Restore(snapshot);
            }
        }

        private static bool TryImport(string directory, ExportFormat? format, out FileCabinetServiceSnapshot snapshot)
        {
            try
            {
                snapshot = Import(directory, format);
                return true;
            }
            catch (InvalidOperationException exception)
            {
                Console.Error.WriteLine(EnglishSource.Cant_deserialize, directory, exception.Message, exception.InnerException?.Message);
            }
            catch (SystemException exception) when (exception is ArgumentException or IOException)
            {
                Console.Error.WriteLine(exception.Message);
            }
            
            snapshot = null;
            return false;
        }

        private static FileCabinetServiceSnapshot Import(string directory, ExportFormat? format)
        {
            var snapshot = new FileCabinetServiceSnapshot();
            using var file = new StreamReader(new FileStream(directory, FileMode.Open));

            switch (format)
            {
                case ExportFormat.Csv:
                    snapshot.LoadFromCsv(file);
                    break;
                case ExportFormat.Xml:
                    snapshot.LoadFromXml(file);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format),
                        string.Format(CultureInfo.CurrentCulture,
                            EnglishSource.ImportCommandHandler_The_format_is_not_supported, format));
            }

            return snapshot;
        }

        private static bool TryGetExportFormat(string source, out ExportFormat? format)
        {
            try
            {
                format = Enum.Parse<ExportFormat>(source, true);
                return true;
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
                format = null;
                return false;
            }
        }
        
        private static bool TryGetDirectory(string source)
        {
            try
            {
                DirectoryIsCorrect(source);
                return true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private static void DirectoryIsCorrect(string directory)
        {
            if (!File.Exists(directory))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, EnglishSource.File_is_not_exist,
                    directory));
            }
        }
    }
}