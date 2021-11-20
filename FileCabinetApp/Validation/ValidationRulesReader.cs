using System;
using System.IO;
using FileCabinetApp.Validation.Criterias;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation
{
    public static class ValidationRulesReader
    {
        /// <summary>
        /// Read <see cref="ValidationRules"/> from JSON file
        /// </summary>
        /// <param name="path">Absolute path to the file with rules</param>
        /// <returns><see cref="ValidationRules"/> object</returns>
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
                Console.Error.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Fill null criterias of the source validation rules with their default values
        /// </summary>
        /// <param name="source">Source validation rule</param>
        /// <returns>Filled <see cref="ValidationRules"/> object</returns>
        private static ValidationRules FillNullParameters(ValidationRules source)
        {
            FillNullDefaultParameters(source);
            FillNullCustomParameters(source);
            return source;
        }

        /// <summary>
        /// Fill null default criterias from source <see cref="ValidationRules"/>
        /// </summary>
        /// <param name="source">Source rules</param>
        private static void FillNullDefaultParameters(ValidationRules source)
        {
            source.Default.FirstName ??= new Name();
            source.Default.LastName ??= new Name();
            source.Default.DateOfBirth ??= new DateOfBirth();
        }
        
        /// <summary>
        /// Fill null custom criterias from source <see cref="ValidationRules"/>
        /// </summary>
        /// <param name="source">Source rules</param>
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