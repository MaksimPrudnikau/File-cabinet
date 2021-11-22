using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;

namespace FileCabinetApp.Handlers
{
    public class UpdateCommandHandler: ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Update;

        public UpdateCommandHandler(IFileCabinetService service) : base(service)
        {
        }

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

            var searchValues = new List<SearchValue>(UpdateLineExtractor.GetSearchValues(request.Parameters));
            var where = UpdateLineExtractor.GetWhereSearchValues(request.Parameters);

            var updated = TryUpdate(searchValues, where, out var recordsId);
            var updatedRecords = string.Join(", #", recordsId);
            if (updated)
            {
                Console.WriteLine(EnglishSource.Records_0_where_updated, updatedRecords);
            }
        }

        private static bool TryUpdate(IEnumerable<SearchValue> values, IList<SearchValue> where, out IEnumerable<int> updated)
        {
            try
            {
                updated = Service.Update(values, where);
                return true;
            }
            catch (SystemException exception) when (exception is ArgumentException or KeyNotFoundException)
            {
                Console.WriteLine(exception.Message);
                updated = null;
                return false;
            }
        }
    }
}