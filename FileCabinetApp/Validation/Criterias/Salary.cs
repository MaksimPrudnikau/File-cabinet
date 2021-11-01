using Newtonsoft.Json;

namespace FileCabinetApp.Validation.Criterias
{
    public class Salary : Criteria<decimal>
    {
        private const decimal DefaultMinValue = 250;
        private const decimal DefaultMaxValue = long.MaxValue;
        
        [JsonProperty("min")] public new decimal MinValue { get; set; } = DefaultMinValue;
        
        [JsonProperty("max")] public new decimal MaxValue { get; set; } = DefaultMaxValue;
    }
}