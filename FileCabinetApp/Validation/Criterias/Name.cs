using Newtonsoft.Json;

namespace FileCabinetApp.Validation.Criterias
{
    public class Name: Criteria<int>
    {
        private const long DefaultMinValue = 2;
        private const long DefaultMaxValue = 100;
        
        [JsonProperty("min")] public new long MinValue { get; set; } = DefaultMinValue;
        
        [JsonProperty("max")] public new long MaxValue { get; set; } = DefaultMaxValue;
    }
}