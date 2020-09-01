using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;

namespace Core.Helper.Extensions
{
    /// <summary>
    /// Object Extensions
    /// </summary>
    public static class ObjectExtensions
    {
        #region Parse
        /// <summary>
        /// Gets the prop value.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="propName">Name of the prop.</param>
        /// <returns></returns>
        public static object GetPropValue(this object src, string propName)
        {
            var item = src.GetType().GetProperty(propName);
            return item != null ? item.GetValue(src, null) : null;
        }

        /// <summary>
        /// Sets the prop value.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="valueSet">The value set.</param>
        public static void SetPropValue(this object src, string propName, object valueSet)
        {
            var obj = src.GetType().GetProperty(propName);
            if (obj != null) obj.SetValue(src, valueSet);
        }

        /// <summary>
        /// Determines whether the specified obj has property.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if the specified obj has property; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Gets the prop value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static T GetPropValue<T>(this Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        /// <summary>
        /// Casts the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static T? Cast<T>(this object obj) where T : struct
        {
            return obj as T?;
        }

        /// <summary>
        /// Finds the long strings.
        /// </summary>
        /// <param name="testObject">The test object.</param>
        public static void FindLongStrings(object testObject)
        {
            var propertyInfo = typeof(EntityObject).GetProperty("MyIntProperty");

            var attribute = (EdmScalarPropertyAttribute)
                            propertyInfo.GetCustomAttributes(
                                    typeof(EdmScalarPropertyAttribute), true)
                            .FirstOrDefault();

            //foreach (PropertyInfo propInfo in testObject.GetType().GetProperties())
            //{
            //    foreach (ColumnAttribute attribute in propInfo.GetCustomAttributes(typeof(ColumnAttribute), true))
            //    {
            //        if (attribute.DbType.ToLower().Contains("varchar"))
            //        {
            //            string dbType = attribute.DbType.ToLower();
            //            int numberStartIndex = dbType.IndexOf("varchar(") + 8;
            //            int numberEndIndex = dbType.IndexOf(")", numberStartIndex);
            //            string lengthString = dbType.Substring(numberStartIndex, (numberEndIndex - numberStartIndex));
            //            int maxLength = 0;
            //            int.TryParse(lengthString, out maxLength);

            //            string currentValue = (string)propInfo.GetValue(testObject, null);

            //            if (!string.IsNullOrEmpty(currentValue) && currentValue.Length > maxLength)
            //                Console.WriteLine(testObject.GetType().Name + "." + propInfo.Name + " " + currentValue + " Max: " + maxLength);

            //        }
            //    }
            //}
        }

        /// <summary>
        /// Gets the primary key object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static PropertyInfo GetPrimaryKeyObject(this Object obj)
        {
            var property = obj.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
            return property;
        }

        /// <summary>
        /// Gets the type of the primary key from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static PropertyInfo GetPrimaryKeyFromType(Type type)
        {
            var property = type.GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
            return property;
        }

        #endregion
    }
}
