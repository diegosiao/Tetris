using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Mapping
{
    internal static class MappingUtils
    {
        internal static string GetTable(object Object)
        {
            var attributes = Object.GetType().GetCustomAttributes(true);

            if (attributes != null)
                foreach (var item in attributes)
                    if (item is DbMappedClass)
                        return ((DbMappedClass)item).Table;

            throw new Exception($"Add the mapping attribute [MappedClass] with table name to '{ Object.GetType().FullName }'");
        }

        internal static bool IsPrimaryKey(PropertyInfo property)
        {
            var attributes = property.GetCustomAttributes(inherit: true);

            if (attributes == null)
                return false;

            foreach (var attribute in attributes)
                if (attribute is DbPrimaryKey)
                    return true;

            return false;
        }

        internal static string GetPrimaryKeyName(object Object)
        {
            foreach (var property in Object.GetType().GetProperties())
            {
                if (IsPrimaryKey(property))
                    return GetPrimaryKeyName(property);
            }

            throw new Exception($"Primary key mapping attribute was not declared for '{ Object.GetType().FullName }'");
        }

        internal static string GetPrimaryKeyName(PropertyInfo property)
        {
            var attributes = property.GetCustomAttributes(inherit: true);

            if (attributes == null)
                throw new Exception($"Primary key mapping attribute was not declared for '{ property.DeclaringType.FullName }'");

            foreach (var attribute in attributes)
            {
                if (attribute is DbPrimaryKey)
                    return string.IsNullOrEmpty(((DbPrimaryKey)attribute).Name) ? property.Name : ((DbPrimaryKey)attribute).Name;
            }

            throw new Exception($"Primary key mapping attribute was not declared for '{ property.DeclaringType.FullName }'");
        }

        internal static object GetIdValue<T>(T Object)
        {
            foreach (var prop in Object.GetType().GetProperties())
            {
                if (IsPrimaryKey(prop))
                    return prop.GetValue(Object);
            }

            throw new Exception($"Primary key mapping attribute was not declared for '{ Object.GetType().FullName }'");
        }


        internal static bool IsCollection(PropertyInfo property)
        {
            return
                property.PropertyType.IsArray ||
                property.PropertyType.GetInterfaces().Contains(typeof(System.Collections.IList));
        }

        internal static bool IsReadOnly(PropertyInfo property)
        {
            var attributes = property.GetCustomAttributes(inherit: true);

            if (attributes == null)
                return false;

            foreach (var attribute in attributes)
                if (attribute is DbReadOnly)
                    return true;

            return false;
        }

        internal static Dictionary<string, object> GetWriteValues(object Object)
        {
            var mappedValues = new Dictionary<string, object>();
            var noPrimaryKey = true;

            foreach (var prop in Object.GetType().GetProperties())
            {
                if (IsReadOnly(prop))
                    continue;

                if (IsCollection(prop))
                    continue;

                if (IsPrimaryKey(prop))
                {
                    noPrimaryKey = false;
                }

                mappedValues.Add(prop.Name, prop.GetValue(Object));
            }

            if (noPrimaryKey)
                throw new Exception($"No primary key mapping attribute for '{ Object.GetType().FullName }' (Attribute missing: [PrimaryKey])");

            return mappedValues;
        }
    }
}
