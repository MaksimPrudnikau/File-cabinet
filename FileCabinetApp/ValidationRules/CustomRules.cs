using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    public class CustomRules : DefaultRules
    {
        [JsonProperty("jobExperience")]
        public JobExperience JobExperience { get; set; }
        
        [JsonProperty("salary")]
        public Salary Salary { get; set; }
        
        [JsonProperty("rank")]
        public Rank Rank { get; set; }
    }
}