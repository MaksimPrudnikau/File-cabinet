using System;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation.Criterias
{
    public class DateOfBirth: Criteria<DateTime>
    {
        private static readonly DateTime DefaultMinValue = new (1950, 1, 1);
        private static readonly DateTime DefaultMaxValue = DateTime.Now;
        
        [JsonProperty("from")] public new DateTime MinValue { get; set; } = DefaultMinValue;

        [JsonProperty("to")] public new DateTime MaxValue { get; set; } = DefaultMaxValue;
    }
}