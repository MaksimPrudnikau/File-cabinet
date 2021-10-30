using FileCabinetApp.Validation.Criterias;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation
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