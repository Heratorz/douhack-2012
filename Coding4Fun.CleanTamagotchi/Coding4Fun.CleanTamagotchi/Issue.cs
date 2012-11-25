using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coding4Fun.CleanTamagotchi
{
    [Serializable]
    public class Issue
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Certainty { get; set; }

        [XmlAttribute]
        public string Level { get; set; }

        [XmlAttribute]
        public string Path { get; set; }
        
        [XmlAttribute]
        public string File { get; set; }

        [XmlAttribute]
        public string Line { get; set; }
    }
}
