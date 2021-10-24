using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    public class Rank
    {
        [JsonProperty("ranks")]
        public char[] Ranks { get; set; }
    }
}