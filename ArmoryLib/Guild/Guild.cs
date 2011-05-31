using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArmoryLib.Deserialization.XmlAttributes;

namespace ArmoryLib.Guild
{
    [XmlArmoryNode("/page/guildInfo/guildHeader")]
    public class Guild
    {
        [XmlArmoryAttribute("name")]
        public string Name { get; set; }

        [XmlArmoryAttribute("realm")]
        public string Realm { get; set; }
    }
}
