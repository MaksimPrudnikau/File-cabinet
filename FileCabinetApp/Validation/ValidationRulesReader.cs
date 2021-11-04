using System;
using System.IO;
using FileCabinetApp.Validation.Criterias;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation
{
    public static class ValidationRulesReader
    {
        public static ValidationRules ReadRules(string path)
        {
            try
            {
                var jsonText = File.ReadAllText(path);
                var model = JsonConvert.DeserializeObject<ValidationRules>(jsonText);
                return FillNullParameters(model);
            }
            catch (SystemException e) when (e is ArgumentException or IOException)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static ValidationRules FillNullParameters(ValidationRules source)
        {
            FillNullDefaultParameters(source);
            FillNullCustomParameters(source);
            return source;
        }

        private static void FillNullDefaultParameters(ValidationRules source)
        {
            source.Default.FirstName ??= new Name();
            source.Default.LastName ??= new Name();
            source.Default.DateOfBirth ??= new DateOfBirth();
        }
        
        private static void FillNullCustomParameters(ValidationRules source)
        {
            source.Custom.FirstName ??= new Name();
            source.Custom.LastName ??= new Name();
            source.Custom.DateOfBirth ??= new DateOfBirth();
            source.Custom.JobExperience ??= new JobExperience();
            source.Custom.Salary ??= new Salary();
            source.Custom.Rank ??= new Rank();
        }
    }
}