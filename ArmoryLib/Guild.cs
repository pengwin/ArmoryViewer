using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ArmoryLib.Deserialization.XmlAttributes;

namespace WowMvc.Models
{
    public class ClassDef
    {
        public virtual int Id { get; private set; }
        public virtual string  Name { get; set; }
        public virtual uint Color { get; set; }
        public virtual string Image { get; set; }

        public ClassDef()
        {
            Name = "Undefined";
            Color = 256;
        }
    }

    [XmlArmoryNode("/page/guildInfo/guildHeader")]
    public class Guild 
    {
        public virtual int Id { get; private set; }

        [XmlArmoryAttribute("name")]
        public virtual string Name { get; set; }

        [XmlArmoryAttribute("realm")]
        public virtual string Realm { get; set; }

        [XmlArmoryArray(XPath = "/page/guildInfo/guild/members")]
        public virtual IList<GuildMember> Players { get; set; }

        public Guild()
        {
            Players = new List<GuildMember>();
        }
    }

    [XmlArmoryInternalNode("character")]
    public class GuildMember
    {
        #region Static constants
        private readonly static string[] _genderStrings = new string[] { "Male", "Female" };
        private readonly static ClassDef[] _classDefs = new ClassDef[] 
        {
            new ClassDef(),
            new ClassDef { Name = "Warrior", Color = 155 } ,
            new ClassDef { Name = "Paladin", Color = 65535 }
        };
        #endregion

        public virtual int Id { get; private set; }

        [XmlArmoryAttribute("name")]
        public virtual string Name { get; set; }

        [XmlArmoryAttribute("achPoints")]
        public virtual int AchievePoints { get; set; }

        [XmlArmoryAttribute("level")]
        public virtual int Level { get; set; }

        private int _classId;

        [XmlArmoryAttribute("classId")]
        public virtual int ClassId 
        {
            get { return _classId; }
            set
            {
                _classId = value;
                if (_classId < _classDefs.Length)
                {
                    Class = _classDefs[_classId];
                }
                else
                {
                    Class = _classDefs[0];
                }
            }
        }

        public virtual ClassDef Class { protected set; get; }

        private int _genderId;

        [XmlArmoryAttribute("genderId")]
        public virtual int GenderId
        {
            get { return _genderId; }
            set
            {
                _genderId = value;
                if (_genderId < _genderStrings.Length)
                {
                    Gender = _genderStrings[_genderId];
                }
                else
                {
                    Gender = "Undefined";
                }
            }
        }

        public virtual string Gender { protected set; get; }

        public virtual Guild Guild { get; set; }
    }

}