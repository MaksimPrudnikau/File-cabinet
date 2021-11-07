using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.FileCabinetService;
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

        private bool TryCreateRecord(SearchValue[] values, out FileCabinetRecord record)
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

        private static FileCabinetRecord CreateRecordFromInsertValue(SearchValue[] value)
        {
            var record = new FileCabinetRecord();
            foreach (var item in value)
            {
                switch (item.Attribute)
                {
                    case SearchValue.SearchAttribute.Id:
                        record.Id = InputConverter.IdConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchAttribute.FirstName:
                        record.FirstName = InputConverter.NameConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchAttribute.LastName:
                        record.LastName = InputConverter.NameConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchAttribute.DateOfBirth:
                        record.DateOfBirth = InputConverter.DateOfBirthConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchAttribute.JobExperience:
                        record.JobExperience = InputConverter.JobExperienceConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchAttribute.Salary:
                        record.Salary = InputConverter.SalaryConverter(item.Value).Result;
                        break;
                    
                    case SearchValue.SearchAttribute.Rank:
                        record.Rank = InputConverter.RankConverter(item.Value).Result;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value));
                }
            }

            return record;
        }

        private static bool TryExtractValues(string parameters, out SearchValue[] values)
        {
            try
            {
                values = ExtractValues(parameters);
                return true;
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                values = null;
                return false;
            }
        }

        private static SearchValue[] ExtractValues(string parameters)
        {
            const string keyword = "values";
            if (!parameters.Contains(keyword, StringComparison.InvariantCulture))
            {
                throw new ArgumentException($"Cannot find key word '{keyword}'");
            }

            var split = RemoveWhiteSpaces(parameters).Split(keyword);
            var keys = GetWords(split[0]);
            var values = GetWords(split[1]);

            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Number of keys doesnt correspond to number of values");
            }

            var created = TryCreateInsertValues(keys, values, out var insertValues);
            return created 
                ? insertValues 
                : null;
        }

        private static bool TryCreateInsertValues(IReadOnlyList<string> keys, IReadOnlyList<string> values, out SearchValue[] insertValues)
        {
            try
            {
                insertValues = CreateInsertValues(keys, values);
                return true;
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                insertValues = null;
                return false;
            }
        }

        private static SearchValue[] CreateInsertValues(IReadOnlyList<string> keys, IReadOnlyList<string> values)
        {
            var insertValues = new List<SearchValue>();
            for (var i = 0; i < keys.Count; i++)
            {
                insertValues.Add(new SearchValue(keys[i], values[i]));
            }

            return insertValues.ToArray();
        }

        private static string RemoveWhiteSpaces(string source)
        {
            return source.Replace(" ", string.Empty, StringComparison.InvariantCulture);
        }

        private static string[] GetWords(string source)
        {
            if (source[0] != '(' || source[^1] != ')')
            {
                throw new ArgumentException("Parameter values should be enclosed in brackets");
            }

            var builder = new StringBuilder(source);
            builder.Replace("(", string.Empty);
            builder.Replace(")", string.Empty);
            builder.Replace("'", string.Empty);
            return builder.ToString().Split(',');
        }
    }
}