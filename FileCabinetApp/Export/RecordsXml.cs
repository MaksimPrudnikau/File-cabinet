using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    [Serializable]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "records")]
    public class RecordsXml
    {
        [XmlElement("record")] 
        public Collection<RecordXml> Records { get; set; }
        
        public RecordsXml(){}

        public RecordsXml(Collection<RecordXml> records)
        {
            Records = records;
        }
    }

    [Serializable]
    public class RecordXml
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        
        [XmlElement("name")]
        public NameXml Name { get; set; } = new();
 
        [XmlElement("dateOfBirth")]
        public string DateOfBirth { get; set; }
        
        [XmlElement("jobExperience")]
        public short JobExperience { get; set; }
        
        [XmlElement("wage")]
        public decimal Wage { get; set; }
 
        [XmlElement("rank")]
        public char Rank { get; set; }
    }
 
    [Serializable]
    public class NameXml
    {
        [XmlAttribute("first")]
        public string First { get; set; }
 
        [XmlAttribute("last")]
        public string Last { get; set; }
    }
}