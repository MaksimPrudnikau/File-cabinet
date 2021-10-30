using System;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation.Criterias
{
    public class DateOfBirth: Criteria<DateTime>
    {
        [JsonProperty("from")]
        public new DateTime MinValue { get; set; }
        
        [JsonProperty("to")]
        public new DateTime MaxValue { get; set; }
    }
}