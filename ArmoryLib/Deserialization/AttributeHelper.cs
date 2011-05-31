using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;

namespace ArmoryLib.Deserialization
{
    public class AttributeHelper<T> where T : Attribute
    {
        public static bool HasAttribute(Type type)
        {
            foreach (object attribute in type.GetCustomAttributes(true))
            {
                if (attribute is T)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasAttribute(PropertyInfo info)
        {
            foreach (object attribute in info.GetCustomAttributes(true))
            {
                if (attribute is T)
                {
                    return true;
                }
            }
            return false;
        }

        public static T GetAttribute(Type type)
        {
            foreach (object attribute in type.GetCustomAttributes(true))
            {
                if (attribute is T)
                {
                    return (T)attribute;
                }
            }
            return null;
        }

        public static T GetAttribute(PropertyInfo info)
        {
            foreach (object attribute in info.GetCustomAttributes(true))
            {
                if (attribute is T)
                {
                    return (T)attribute;
                }
            }
            return null;
        }

    }
}
