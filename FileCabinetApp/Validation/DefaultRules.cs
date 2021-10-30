using FileCabinetApp.Validation.Criterias;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation
{
    public class DefaultRules
    {
        [JsonProperty("firstName")]
        public Name FirstName { get; set; }
        
        [JsonProperty("lastName")]
        public Name LastName { get; set; }
        
        [JsonProperty("dateOfBirth")]
        public DateOfBirth DateOfBirth { get; set; }
    }
}