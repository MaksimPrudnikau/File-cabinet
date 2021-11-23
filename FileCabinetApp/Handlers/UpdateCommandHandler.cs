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

        /// <summary>
        /// Update suitable records with input values
        /// </summary>
        /// <param name="request">Source command request</param>
        /// <exception cref="ArgumentNullException">Source request is null</exception>
        /// <example>
        /// update set firstname = 'John', lastname = 'Doe' , dateofbirth = '5/18/1986' where id = '1'
        /// update set DateOfBirth='05/18/1986' where FirstName = 'Stan' and LastName='Smith'
        /// </example>
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
            if (!updated)
            {
                return;
            }
            
            var updatedRecords = string.Join(", #", recordsId);
            Console.WriteLine(EnglishSource.Records_0_where_updated, updatedRecords);
        }

        private static bool TryUpdate(IList<SearchValue> values, IList<SearchValue> where, out IEnumerable<int> updated)
        {
            try
            {
                updated = Service.Update(values, where);
                return true;
            }
            catch (SystemException exception) when (exception is ArgumentException or KeyNotFoundException)
            {
                Console.Error.WriteLine(exception.Message);
                updated = null;
                return false;
            }
        }
    }
}