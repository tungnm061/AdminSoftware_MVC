using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Core.Data
{
    public static class DataReaderExtensions
    {
        public static List<T> MapToList<T>(this DbDataReader dr) where T : new()
        {
            if (dr != null && dr.HasRows)
            {
                var entity = typeof(T);
                var entities = new List<T>();
                var propDict = new Dictionary<string, PropertyInfo>();
                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

                while (dr.Read())
                {
                    T newObject = new T();
                    for (int index = 0; index < dr.FieldCount; index++)
                    {
                        var columname = dr.GetName(index).ToUpper();
                        if (columname.IndexOf("_") > 0)
                        {
                            columname = columname.Replace("_", "").Trim();
                        }
                        if (propDict.ContainsKey(columname))
                        {
                            var info = propDict[columname];
                            if ((info != null) && info.CanWrite)
                            {
                                var val = dr.GetValue(index);
                                info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                            }
                        }
                    }
                    entities.Add(newObject);
                }
                return entities;
            }
            return null;
        }

        public static DataTable ToDataTable<T>(IList<T> data)// T is any generic type
        {
            DataTable table = new DataTable();
            if (data != null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));


                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                object[] values = new object[props.Count];
                if (data != null)
                {
                    foreach (T item in data)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item);
                        }
                        table.Rows.Add(values);
                    }
                }
            }


            return table;
        }
    }
}
