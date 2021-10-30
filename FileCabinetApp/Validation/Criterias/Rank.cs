using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation.Criterias
{
    public class Rank
    {
        [JsonProperty("ranks")] public Collection<char> Ranks { get; } = new();
    }
}