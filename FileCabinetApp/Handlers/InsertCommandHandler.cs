using System;
using System.Collections.Generic;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.Handlers.Helpers;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Handlers
{
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private const RequestCommand Command  = RequestCommand.Insert;
        private readonly IRecordValidator _validator = new ValidationBuilder().CreateCustom();

        public InsertCommandHandler(IFileCabinetService service) : base(service)
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
        /// <param name="parameters"></param>
        /// <param name="values"></param>
        /// <returns></returns>
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