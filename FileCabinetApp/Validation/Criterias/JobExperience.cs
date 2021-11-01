using FileCabinetApp.FileCabinetService;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation.Criterias
{
    public class JobExperience: Criteria<short>
    {
        private const short DefaultMinValue = 0;
        private const short DefaultMaxValue = 70;
        
        [JsonProperty("min")] public new short MinValue { get; set; } = DefaultMinValue;
        
        [JsonProperty("max")] public new short MaxValue { get; set; } = DefaultMaxValue;
    }
}