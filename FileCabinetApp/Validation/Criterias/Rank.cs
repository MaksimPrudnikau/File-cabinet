using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation.Criterias
{
    public class Rank
    {
        private static readonly Collection<char> Grades = new() {'F', 'D', 'C', 'B', 'A'};
        
        [JsonProperty("ranks")] public Collection<char> Ranks { get; } = Grades;
    }
}