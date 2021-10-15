using System;
using System.Collections.Generic;

namespace FileCabinetApp.Handlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter _printer;

        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer) : base(service)
        {
            _printer = printer;
        }
        
        public override void SetNext(ICommandHandler handler)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public override void Handle(AppCommandRequest request)
        {
            const int attributeIndex = 0;
            const int searchValueIndex = 1;
            var inputs = request.Parameters.Split(' ', 2);
            var attribute = inputs[attributeIndex];
            var searchValue = inputs[searchValueIndex];
            
            try
            {
                _printer.Print(FindByAttribute(attribute, searchValue));
            }
            catch (Exception exception) when (exception is ArgumentException or FormatException)
            {
                Console.Error.WriteLine(exception.Message);
            }
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