using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

using System.Reflection;
using System.Collections;

using ArmoryLib.Deserialization.XmlAttributes;

namespace ArmoryLib.Deserialization
{
    /// <summary>
    /// Set property value according to value of xml node attribute
    /// </summary>
    class XmlPropertyParser
    {
        private XmlNode _node;
        private XmlArmoryAttributeAttribute _attr;
        private object _subject;
        private PropertyInfo _info;

        public XmlPropertyParser(XmlArmoryAttributeAttribute attribute, object subject, PropertyInfo info)
        {
            _attr = attribute;
            _subject = subject;
            _info = info;
        }

        public void GetNode(XmlDocument doc)
        {
            _node = doc.SelectSingleNode(_attr.XPath);
            if (_node == null)
            {
                throw new ArgumentException("Cant find node with " + _attr.XPath + " XPath");
            }
        }

        private void SetPropertyValue()
        {
            string xmlAttrValue = _node.Attributes[_attr.Name].Value;
            if (_info.PropertyType == typeof(System.Int32))
            {
                _info.SetValue(_subject, Convert.ToInt32(xmlAttrValue), null);
            }
            else if (_info.PropertyType == typeof(System.String))
            {
                _info.SetValue(_subject, xmlAttrValue, null);
            }
            else
            {
                throw new ArgumentException("Type of " + _info.Name + " " + _info.GetType().ToString() + " is not valid field type. Allowed only String, Int32");
            }
        }

        public void SetProperty()
        {

            if (_node.Attributes != null)
            {
                if (_node.Attributes[_attr.Name] != null)
                {
                    SetPropertyValue();
                }
                else
                {
                    throw new ArgumentException("No node with name: " + _attr.Name);
                }
            }
            else
            {
                throw new ArgumentException("Node " + _node.Name + " has no attributes");
            }
        }
    }
}
