using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;

namespace FileCabinetApp.Handlers
{
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Delete;
        private const string Keyword = "where";
        
        public DeleteCommandHandler(IFileCabinetService service) : base(service)
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
            var deleted = TryDelete(request.Parameters, out var deletedId);
            if (!deleted)
            {
                return;
            }

            Console.WriteLine(EnglishSource.Records_are_deleted, string.Join(", #", deletedId));
        }

        private static bool TryDelete(string parameters, out IEnumerable<int> deleted)
        {
            try
            {
                var searchValue = GetSearchAttribute(parameters);
                deleted = Service.Delete(searchValue);
                if (deleted is null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        EnglishSource.Suitable_records_not_found, searchValue.Property, searchValue.Value));
                }

                return true;
            }
            catch (Exception exception) when (exception is OverflowException or FormatException
                or ArgumentException or KeyNotFoundException)
            {
                Console.Error.WriteLine(exception.Message);
                deleted = null;
                return false;
            }
        }

        /// <summary>
        /// Extract <see cref="SearchValue"/> from source parameters
        /// </summary>
        /// <param name="parameters">Source parameters</param>
        /// <returns><see cref="SearchValue"/> extracted from source string</returns>
        /// <exception cref="ArgumentException">Cant find delimiter <see cref="Keyword"/></exception>
        private static SearchValue GetSearchAttribute(string parameters)
        {
            if (!parameters.Contains(Keyword, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException($"Cant find keyword '{Keyword}'");
            }

            var keywordIndex = parameters.IndexOf(Keyword, StringComparison.InvariantCultureIgnoreCase) + Keyword.Length + 1;
            
            return DefaultLineExtractor.ExtractSearchValues(parameters[keywordIndex..])[0];
        }
    }
}