using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Handlers
{
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        public override RequestCommand Command => RequestCommand.Insert;
        
        private readonly IRecordValidator _validator = new ValidationBuilder().CreateCustom();

        public InsertCommandHandler(IFileCabinetService service) : base(service)
        {
        }

        /// <summary>
        /// Insert a new value to the current service. The value is inserted before the first record with a greater ID.
        /// The identificator, first name, last name and date of birth values are required because the are initial for
        /// any record. Number of the properties must be correspond with values. Quotes in values are optional
        /// </summary>
        /// <param name="request">Source command request</param>
        /// <exception cref="ArgumentNullException">Request is null</exception>
        /// <example>
        /// insert (id, firstname, lastname, dateofbirth) values ('1', 'John', 'Doe', '05/18/1986')
        /// insert (firstname, id, lastname, dateofbirth, rank) values (Ivan, 4, Ivanov, 13/05/2001, A)
        /// </example>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var extracted = TryExtractValues(request.Parameters, out var values);
            if (!extracted) return;
            
            var created = TryCreateRecord(values, out var record);
            if (!created) return;
            
            Service.Insert(record);
        }

        private bool TryCreateRecord(IEnumerable<SearchValue> values, out FileCabinetRecord record)
        {
            try
            {
                record = CreateRecordFromInsertValue(values);
                _validator.Validate(record);
                return true;
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                record = null;
                return false;
            }
        }

        /// <summary>
        /// Create a new <see cref="FileCabinetRecord"/> and fill it with source values
        /// </summary>
        /// <param name="value">An array of values to fill</param>
        /// <returns><see cref="FileCabinetRecord"/> record</returns>
        private static FileCabinetRecord CreateRecordFromInsertValue(IEnumerable<SearchValue> value)
        {
            var record = new FileCabinetRecord();
            foreach (var item in value)
            {
                switch (item.Property)
                {
                    case SearchValue.SearchProperty.Id:
                        record.Id = InputConverter.IdConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchProperty.FirstName:
                        record.FirstName = InputConverter.NameConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchProperty.LastName:
                        record.LastName = InputConverter.NameConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchProperty.DateOfBirth:
                        record.DateOfBirth = InputConverter.DateOfBirthConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchProperty.JobExperience:
                        record.JobExperience = InputConverter.JobExperienceConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchProperty.Salary:
                        record.Salary = InputConverter.SalaryConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchProperty.Rank:
                        record.Rank = InputConverter.RankConverter(item.Value).Result;
                        break;
                }
            }

            return record;
        }

        /// <summary>
        /// Try extract 
        /// </summary>
        /// <param name="parameters">Source parameters line</param>
        /// <param name="values">Values to extract</param>
        /// <returns>True if extracting was successful</returns>
        private static bool TryExtractValues(string parameters, out ICollection<SearchValue> values)
        {
            try
            {
                values = InsertLineExtractor.ExtractValues(parameters);
                return true;
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                values = null;
                return false;
            }
        }
    }
}