using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

using System.Reflection;
using System.Collections;

namespace ArmoryLib.Deserialization.XmlAttributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class XmlArmoryNodeAttribute : Attribute
    {
        public string XPath { get; set; }

        public XmlArmoryNodeAttribute(string XPath)
        {
            this.XPath = XPath;
        }

        public override string ToString()
        {
            return XPath;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class XmlArmoryInternalNodeAttribute : Attribute
    {
        public string Name { get; set; }

        public XmlArmoryInternalNodeAttribute(string Name)
        {
            this.Name = Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class XmlArmoryAttributeAttribute : Attribute
    {
        public string Name { get; set; }

        public XmlArmoryAttributeAttribute(string Name)
        {
            this.Name = Name;
        }

        public override string ToString()
        {
            return "";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class XmlArmoryArrayAttribute : Attribute
    {
        public string XPath { get; set; }

        public override string ToString()
        {
            return "";
        }
    }




}
