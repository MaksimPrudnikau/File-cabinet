using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    public abstract class Criteria<T>
    {
        [JsonProperty("min")]
        public T MinValue { get; set; }
        
        [JsonProperty("max")]
        public T MaxValue { get; set; }
    }
}