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
    public class XmlArmoryParser
    {
        /// <summary>
        /// Input xml document
        /// </summary>
        private XmlDocument doc;
        /// <summary>
        /// Type of output object
        /// </summary>
        private Type type;
        /// <summary>
        /// Output object from xml document
        /// </summary>
        private object subject;
        
        public XmlArmoryParser(Type type, XmlDocument doc)
        {
            this.doc = doc;
            this.type = type;
            this.subject = null;
        }

        private T GetPropertyAttribute<T>( PropertyInfo property)  where T :  Attribute
        {
            object[] attributes = property.GetCustomAttributes(typeof(T), false);
            if (attributes == null)
            {
                return default(T);
            }
            else
            {
                if (attributes.Length > 1)
                {
                    throw new ArgumentException("Property " + property.Name + " has more than one attribute " + typeof(T).Name);
                }
                return (T) attributes[0];
            }
        }

        /// <summary>
        /// Parse xml document and get data from
        /// </summary>
        /// <returns></returns>
        public object GetData()
        {
            // get object constructors
            ConstructorInfo[] ci = type.GetConstructors();
            ConstructorInfo defaultConstructor = null;
            foreach (ConstructorInfo c in ci)
            {
                if (c.GetParameters().Count() == 0) //found default ctor with no params
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
                subject = defaultConstructor.Invoke(new Object[0]); // create object. call default constructor

                foreach (PropertyInfo info in type.GetProperties() ) // process object property list
                {
                    XmlArmoryAttributeAttribute attr = GetPropertyAttribute<XmlArmoryAttributeAttribute>(info);
                    if (attr != null)
                    {
                        XmlPropertyParser propParser = new XmlPropertyParser(attr,subject,info);
                        propParser.GetNode(doc);
                        propParser.SetProperty();
                    }
                }
                return subject;
            }
            //return null; 

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
                            XmlArmoryParser ds = new XmlArmoryParser();
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


    }
}
