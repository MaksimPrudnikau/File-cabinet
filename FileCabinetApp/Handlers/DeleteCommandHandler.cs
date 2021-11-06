using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.FileCabinetService;

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
                var attribute = GetSearchAttribute(request.Parameters);
                var value = GetValue(request.Parameters);
                var searchValue = new SearchValue(attribute, value);
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

        private static string GetSearchAttribute(string parameters)
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

            parameters = new StringBuilder(parameters).Replace(" ", string.Empty).ToString();
            return parameters.Split("where")[1].Split('=')[0];
        }

        private static string GetValue(string parameters)
        {
            var firstLetter = parameters.IndexOf('\'', StringComparison.InvariantCulture) + 1;
            var lastLetter = parameters.LastIndexOf('\'');
            return parameters[firstLetter..lastLetter];
        }
    }
}