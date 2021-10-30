using Newtonsoft.Json;

namespace FileCabinetApp.Validation.Criterias
{
    public abstract class Criteria<T>
    {
        [JsonProperty("min")]
        public T MinValue { get; set; }
        
        [JsonProperty("max")]
        public T MaxValue { get; set; }
    }
}