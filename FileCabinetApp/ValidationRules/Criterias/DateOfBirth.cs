using System;
using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    public class DateOfBirth: Criteria<DateTime>
    {
        [JsonProperty("from")]
        public new DateTime MinValue { get; set; }
        
        [JsonProperty("to")]
        public new DateTime MaxValue { get; set; }
    }
}