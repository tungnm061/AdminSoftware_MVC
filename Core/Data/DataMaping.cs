using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Core.Data.Attributes;

namespace Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DataMaping
    {
        /// <summary>
        /// Maps the data to business entity collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr">The dr.</param>
        /// <returns></returns>
        public static List<T> MapDataToBusinessEntityCollection<T>(IDataReader dr) where T : new()
        {
            var businessEntityType = typeof(T);
            var entitys = new List<T>();
            //var hashTable = LoadProperties<T>();
            var hashTable = new Hashtable();
            var properties = businessEntityType.GetProperties();

            foreach (var info in properties)
            {
                var attrsProperty = info.GetCustomAttributes();
                var hashName = info.Name.ToUpper();
                foreach (var attr in attrsProperty)
                {
                    if (attr is FieldMapAttribute)
                    {
                        var oField = (FieldMapAttribute)attr;
                        hashName = oField.Column.ToUpper();
                    }
                }
                hashTable[hashName] = info;
            }
            while (dr.Read())
            {
                var newObject = new T();
                for (var index = 0; index < dr.FieldCount; index++)
                {
                    var info = (PropertyInfo)hashTable[dr.GetName(index).ToUpper()];
                    if ((info != null) && info.CanWrite)
                    {
                        try
                        {
                            info.SetValue(newObject, dr.GetValue(index), null);
                        }
                        catch
                        {
                        }
                    }
                }
                entitys.Add(newObject);
            }
            dr.Close();
            return entitys;
        }

        /// <summary>
        /// Loads the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Hashtable LoadProperties<T>()
        {
            Type businessEntityType = typeof(T);
            Hashtable hashTable = new Hashtable();
            PropertyInfo[] properties = businessEntityType.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo info = properties[i];
                hashTable[info.Name.ToUpper()] = info;
            }
            return hashTable;
        }
    }
}
