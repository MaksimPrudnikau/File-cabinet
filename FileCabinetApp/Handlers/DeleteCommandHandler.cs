using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;

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
                var searchValue = GetSearchAttribute(request.Parameters);
                var deleted = string.Join(", #", Service.Delete(searchValue));
                
                Console.WriteLine(EnglishSource.Records_are_deleted, deleted);
            }
            catch (Exception exception) when (exception is OverflowException or FormatException or ArgumentException)
            {
                Console.Error.WriteLine(exception.Message);
            }
            catch (KeyNotFoundException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }

        private static SearchValue GetSearchAttribute(string parameters)
        {
            const string keyword = "where";
            if (!parameters.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException($"Cant find keyword '{keyword}'");
            }

            if (!parameters.Contains("=", StringComparison.Ordinal))
            {
                throw new ArgumentException("Cannot find equal sign");
            }

            var keywordIndex = parameters.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) + keyword.Length + 1;
            return Extractor.ExtractSearchValues(parameters[keywordIndex..])[0];
        }
    }
}