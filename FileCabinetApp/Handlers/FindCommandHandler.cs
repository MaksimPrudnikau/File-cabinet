using System;
using System.Collections.Generic;

namespace FileCabinetApp.Handlers
{
    public class FindCommandHandler : CommandHandlerBase
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
        /// Prints all the records with entered attribute equals searchValue
        /// </summary>
        /// <param name="parameters">Parameter in format "attribute searchValue"</param>
        private void Find(string parameters)
        {
            const int attributeIndex = 0;
            const int searchValueIndex = 1;
            var inputs = parameters.Split(' ', 2);
            var attribute = inputs[attributeIndex];
            var searchValue = inputs[searchValueIndex];

            try
            {
                foreach (var record in FindByAttribute(attribute, searchValue))
                {
                    record.Print();
                }
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
                "FIRSTNAME" => Program.Service.FindByFirstName(searchValue),
                "LASTNAME" => Program.Service.FindByLastName(searchValue),
                "DATEOFBIRTH" => Program.Service.FindByDateOfBirth(searchValue),
                _ => throw new ArgumentException("Entered attribute is not exist")
            };
        }
    }
}