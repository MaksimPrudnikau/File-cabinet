using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
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