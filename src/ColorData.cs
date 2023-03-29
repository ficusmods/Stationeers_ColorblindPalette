using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ColorblindPalette
{
    [XmlRoot]
    public class ColorData
    {
        [XmlElement]
        public float R;
        [XmlElement]
        public float G;
        [XmlElement]
        public float B;
    }
}
