using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ColorblindPalette
{
    [XmlRoot]
    public class ColorblindConfig
    {
        [XmlArray]
        public List<ColorSwapData> ColorSwaps;
        [XmlArrayItem("Structure")]
        public List<string> StructureFilter;
    }
}
