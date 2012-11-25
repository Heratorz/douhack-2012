using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coding4Fun.CleanTamagotchi
{
    [Serializable]
    public class Message
    {
        [XmlAttribute]
        public string TypeName { get; set; }

        [XmlAttribute]
        public string Category { get; set; }

        [XmlAttribute]
        public string CheckId { get; set; }

        [XmlAttribute]
        public string Status { get; set; }

        [XmlAttribute]
        public string Created { get; set; }

        [XmlAttribute]
        public string FixCategory { get; set; }

        [XmlElement]
        public Issue Issue { get; set; }
    }
}
