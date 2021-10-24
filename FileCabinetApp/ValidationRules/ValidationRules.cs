using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    public class ValidationRules
    {
        [JsonProperty("default")]
        public DefaultRules Default { get; set; }
        
        [JsonProperty("custom")]
        public CustomRules Custom { get; set; }
    }
}