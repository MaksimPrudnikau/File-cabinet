using System;
using System.IO;
using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    public static class ValidationRulesReader
    {
        public static ValidationRules ReadRules(string path)
        {
            try
            {
                var jsonText = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<ValidationRules>(jsonText);
            }
            catch (SystemException e) when (e is ArgumentException or IOException)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}