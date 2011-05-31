using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ArmoryLib.Deserialization.XmlAttributes;

namespace WowMvc.Models
{
    [XmlArmoryNode("/page/guildInfo/guildHeader")]
    public class Guild
    {
        public int GuildId { get; set; }

        [XmlArmoryAttribute("name")]
        public string Name { get; set; }

        [XmlArmoryAttribute("realm")]
        public string Realm { get; set; }

        [XmlArmoryArray(XPath = "/page/guildInfo/guild/members")]
        public virtual ICollection<GuildMember> Players { get; set; }
    }
    [XmlArmoryInternalNode("character")]
    public class GuildMember
    {
        public int GuildMemberId { get; set; }

        [XmlArmoryAttribute("name")]
        public string Name { get; set; }

        [XmlArmoryAttribute("achPoints")]
        public int AchievePoints { get; set; }

        [XmlArmoryAttribute("level")]
        public int Level { get; set; }

        private int _genderId;
        [XmlArmoryAttribute("genderId")]
        public int GenderId
        {
            get
            {
                return _genderId;
            }
            set
            {
                _genderId = value;
                if (_genderId == 0)
                {
                    _gender = "Муж";
                }
                else if (_genderId == 1)
                {
                    _gender = "Жен";
                }
                else
                {
                    _gender = "Неопределен";
                }
            }
        }

        private string _gender = "";
        public string Gender 
        {
            get
            {
                return _gender;
            }
        }
        


        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }
    }

}