using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JayLib.JaySerialization
{
    public static class SerializerHelper
    {
        public static string GetClassName(object _Object)
        {
            return _Object?.GetType().Name;
        }

        public static PropertyInfo[] GetProperties(object _Object)
        {
            return _Object?.GetType().GetProperties();
        }

        public static FieldInfo[] GetField(object _Object)
        {
            return _Object.GetType().GetFields();
        }

        public static void SerializationPropertyHelper(SerializationInfo info, object _Object, params Type[] notSerializeTypes)
        {
            if (notSerializeTypes == null)
            {
                foreach (PropertyInfo property in GetProperties(_Object))
                {
                    if (property.CanWrite && property.CanRead && property.PropertyType is ISerializable)
                    {
                        info.AddValue(property.Name, property.GetValue(_Object));
                    }
                }
            }
            else
            {
                foreach (PropertyInfo property in GetProperties(_Object))
                {
                    if (property.CanWrite && property.CanRead && property.PropertyType is ISerializable && !notSerializeTypes.Contains(property.PropertyType))
                    {
                        info.AddValue(property.Name, property.GetValue(_Object));
                    }
                }
            }
        }

        public static void DeserializtionPropertyHelper(SerializationInfo info, object _Object, params Type[] notSerializeTypes)
        {
            if (notSerializeTypes == null)
            {
                foreach (PropertyInfo property in GetProperties(_Object))
                {
                    if (property.CanWrite && property.CanRead && property.PropertyType is ISerializable)
                    {
                        property.SetValue(_Object, info.GetValue(property.Name, property.PropertyType));
                    }
                }
            }
            else
            {

                foreach (PropertyInfo property in GetProperties(_Object))
                {
                    if (property.CanWrite && property.CanRead && property.PropertyType is ISerializable && !notSerializeTypes.Contains(property.PropertyType))
                    {
                        property.SetValue(_Object, info.GetValue(property.Name, property.PropertyType));
                    }
                }
            }
        }

        public static void SerializationFieldHelper(SerializationInfo info, object _Object)
        {
            foreach (FieldInfo field in GetField(_Object))
            {
                if (field.IsPublic && !field.IsNotSerialized && !field.IsStatic)
                {
                    info.AddValue(field.Name, field.GetValue(_Object));
                }
            }
        }

        public static void DeSerializationFieldHelper(SerializationInfo info, object _Object)
        {
            foreach (FieldInfo field in GetField(_Object))
            {
                if (field.IsPublic && !field.IsNotSerialized && !field.IsStatic)
                {
                    field.SetValue(_Object, info.GetValue(field.Name, field.FieldType));
                }
            }
        }
    }
}
