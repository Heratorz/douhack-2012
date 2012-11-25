using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coding4Fun.CleanTamagotchi
{
    [Serializable]
    public class Info
    {
        [XmlArray("Messages")]
        [XmlArrayItem("Message")]
        public Message[] Messages { get; set; }

        public bool IsGreen { get; set; }
    }
}
