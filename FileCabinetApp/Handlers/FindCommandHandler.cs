using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.FileCabinetService.Iterators;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Handlers
{
    
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command = RequestCommand.Find;
        private readonly Action<IEnumerable<FileCabinetRecord>> _print;

        public FindCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> print) :
            base(service)
        {
            _print = print;
        }

        /// <summary>
        /// Prints all records with suitable attribute
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
            
            const int attributeIndex = 0;
            const int searchValueIndex = 1;
            var inputs = request.Parameters.Split(' ', 2);
            var attribute = inputs[attributeIndex];
            var searchValue = inputs[searchValueIndex];
            
            _print(FindByAttribute(attribute, searchValue));
            }

        /// <summary>
        /// Create an array where any attribute element equals searchValue
        /// </summary>
        /// <param name="attribute">Search property</param>
        /// <param name="searchValue">Value to search</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Entered attribute is not exist</exception>
        private static IEnumerable<FileCabinetRecord> FindByAttribute(string attribute, string searchValue)
        {
            var created = TryCreateIterator(attribute, searchValue, out var iterator);
            var records = new List<FileCabinetRecord>();
            if (created)
            {
                while (iterator.HasMore())
                {
                    var read = iterator.GetNext();
                    if (read is not null)
                    {
                        records.Add(read);
                    }
                }
            }

            return records;
        }

        private static bool TryCreateIterator(string attribute, string searchValue, out IRecordIterator iterator)
        {
            try
            {
                iterator = CreateIterator(attribute, searchValue);
                return true;
            }
            catch (SystemException exception) when (exception is ArgumentNullException or KeyNotFoundException)
            {
                iterator = null;
                Console.Error.WriteLine(exception.Message);
                return false;
            }
        }

        private static IRecordIterator CreateIterator(string attribute, string searchValue)
        {
            return attribute.ToUpperInvariant() switch
            {
                "FIRSTNAME" => Service.FindByFirstName(searchValue),
                "LASTNAME" => Service.FindByLastName(searchValue),
                "DATEOFBIRTH" => Service.FindByDateOfBirth(searchValue),
                _ => throw new ArgumentException("Entered attribute is not exist")
            };
        }
    }
}