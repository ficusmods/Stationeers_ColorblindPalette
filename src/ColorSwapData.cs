using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ColorblindPalette
{
    [XmlRoot]
    public class ColorSwapData
    {
        [XmlElement]
        public string ColorNameOriginal;
        [XmlElement]
        public ColorData EmissionColor;
        [XmlElement]
        public float EmissionIntensity;
        [XmlElement]
        public string TextureNew;
    }
}
