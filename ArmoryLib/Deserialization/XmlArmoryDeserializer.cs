using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

using System.Reflection;
using System.Collections;

using ArmoryLib.Deserialization.XmlAttributes;

namespace ArmoryLib.Deserialization
{
    public class XmlArmoryDeserializer
    {
        private object subject = null;

        private XmlNode node = null;

        private Type type;

        private XmlDocument doc;

        public void Init(Type type, XmlDocument doc)
        {
            string xPath = "";
            this.type = type;
            XmlArmoryNodeAttribute attr = AttributeHelper<XmlArmoryNodeAttribute>.GetAttribute(type);
            if (attr != null)
            {
                xPath = attr.XPath;
                this.node = GetNode(doc, xPath);
                if (node == null)
                {
                    throw new ArgumentException("Cant find node with this XPath");
                }
                this.doc = doc;
            }
            else
            {
                throw new ArgumentException("Object has no XmlArmoryNodeAttribute or XPath parameter not set");
            }
        }

        public void Init(Type type, XmlNode node)
        {
            this.type = type;
            this.node = node;
        }

        private XmlNode GetNode(XmlDocument doc, string XPath)
        {
            XmlNode temp = doc.SelectSingleNode(XPath);
            return temp;
        }

        private void SetNode(PropertyInfo info)
        {
            if (AttributeHelper<XmlArmoryNodeAttribute>.GetAttribute(info.PropertyType) != null)
            {
                XmlArmoryDeserializer ds = new XmlArmoryDeserializer();
                ds.Init(info.PropertyType, doc);
                object temp = ds.Deserialize();
                info.SetValue(subject, temp, null);
            }
        }

        private void SetProperty(PropertyInfo info)
        {
            XmlArmoryAttributeAttribute attr = AttributeHelper<XmlArmoryAttributeAttribute>.GetAttribute(info);
            if (node.Attributes != null)
            {
                if (node.Attributes[attr.Name] != null)
                {
                    string attrValue = node.Attributes[attr.Name].Value;
                    if (info.PropertyType == typeof(System.Int32))
                    {
                        info.SetValue(subject, Convert.ToInt32(attrValue), null);
                    }
                    else if (info.PropertyType == typeof(System.String))
                    {
                        info.SetValue(subject, attrValue, null);
                    }
                    else
                    {
                        throw new ArgumentException("Type of " + info.Name + " " + info.GetType().ToString() + " is not valid field type. Allowed only String,Int32");
                    }
                }
                else
                {
                    throw new ArgumentException("No node with name: " + attr.Name);
                }
            }
            else
            {
                throw new ArgumentException("Node has no attributes");
            }
        }

        private void SetArray(PropertyInfo info)
        {
            if (info.PropertyType.IsGenericType)
            {
                Type iface = info.PropertyType;
                if (iface.Name == "ICollection`1")
                {
                    Type[] colParameters = info.PropertyType.GetGenericArguments(); // get collection parameters
                    Type itemType = colParameters[0]; // get type T
                    if (AttributeHelper<XmlArmoryInternalNodeAttribute>.HasAttribute(itemType)) // T has attribute
                    {
                        // Create List<T> object
                        Type listType = typeof(List<>);
                        Type[] typeArgs = { itemType }; // argument T for collection List<T>
                        Type colType = listType.MakeGenericType(typeArgs);
                        object list = Activator.CreateInstance(colType); // same as new List<T>()

                        XmlArmoryInternalNodeAttribute attr = AttributeHelper<XmlArmoryInternalNodeAttribute>.GetAttribute(itemType); // get attribute
                        XmlArmoryArrayAttribute arrayAttribute = AttributeHelper<XmlArmoryArrayAttribute>.GetAttribute(info);
                        // populating list with Objects
                        string tempXPath = @"//" + attr.Name;
                        if ((arrayAttribute.XPath != null) && (arrayAttribute.XPath != ""))
                        {
                            tempXPath = arrayAttribute.XPath + @"//" + attr.Name;
                        }
                        foreach (XmlNode n in node.SelectNodes(tempXPath))
                        {
                            XmlArmoryDeserializer ds = new XmlArmoryDeserializer();
                            ds.Init(itemType, n);
                            object colItem = ds.Deserialize(); // deserialize object from node
                            Object[] addParams = { colItem };
                            MethodInfo addMethod = iface.GetMethod("Add");
                            addMethod.Invoke(list, addParams);  // same as list.Add(colItem);
                        }

                        info.SetValue(subject, list, null);
                    }
                }

            }
        }

        public object Deserialize()
        {
            // get object constructors
            ConstructorInfo[] ci = type.GetConstructors();
            ConstructorInfo defaultConstructor = null;
            foreach (ConstructorInfo c in ci)
            {
                if (c.GetParameters().Count() == 0) //have found default ctor with no params
                {
                    defaultConstructor = c;
                }
            }

            if (defaultConstructor == null)
            {
                throw new ArgumentException("Type " + type.Name + " has no default constructor with no parameters");
            }
            else
            {
                subject = defaultConstructor.Invoke(new Object[0]); // create object

                foreach (PropertyInfo info in (type.GetProperties()))
                {
                    if (AttributeHelper<XmlArmoryAttributeAttribute>.HasAttribute(info))
                    {
                        SetProperty(info);
                    }
                    if (AttributeHelper<XmlArmoryNodeAttribute>.HasAttribute(info.PropertyType))
                    {
                        if (doc != null)
                        {
                            SetNode(info);
                        }
                    }
                    if (AttributeHelper<XmlArmoryArrayAttribute>.HasAttribute(info))
                    {
                        SetArray(info);
                    }
                }
                return subject;
            }
            return null; // if exception throw code will be deleted

        }

    }
}
