using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Data
{
    public class ObjectDataReader<TData> : IDataReader
    {
        /// <summary>
        /// The enumerator for the IEnumerable{TData} passed to the constructor for 
        /// this instance.
        /// </summary>
        private IEnumerator<TData> dataEnumerator;

        /// <summary>
        /// The lookup of accessor functions for the properties on the TData type.
        /// </summary>
        private Func<TData, object>[] accessors;

        /// <summary>
        /// The lookup of property names against their ordinal positions.
        /// </summary>
        private Dictionary<string, int> ordinalLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDataReader&lt;TData&gt;"/> class.
        /// </summary>
        /// <param name="data">The data this instance should enumerate through.</param>
        public ObjectDataReader(IEnumerable<TData> data)
        {
            dataEnumerator = data.GetEnumerator();

            // Get all the readable properties for the class and
            // compile an expression capable of reading it
            var propertyAccessors = typeof(TData)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                //.Where(p => p.CanRead)
                .Where(p => p.CanRead && !p.GetMethod.IsVirtual)
                .Where(p =>
                {
                    var attributes = p.GetCustomAttributes(false);
                    return !attributes.Any(a => a is KeyAttribute)
                        && !attributes.Any(a => a is NotMappedAttribute)
                        ;
                })
                .Select((p, i) => new
                    {
                        Index = i,
                        Property = p,
                        Accessor = CreatePropertyAccessor(p)
                    })
                .ToArray();

            accessors = propertyAccessors.Select(p => p.Accessor).ToArray();
            ordinalLookup = propertyAccessors.ToDictionary(
                p => p.Property.Name,
                p => p.Index,
                StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Creates a property accessor for the given property information.
        /// </summary>
        /// <param name="p">The property information to generate the accessor for.</param>
        /// <returns>The generated accessor function.</returns>
        private Func<TData, object> CreatePropertyAccessor(PropertyInfo p)
        {
            // Define the parameter that will be passed - will be the current object
            var parameter = Expression.Parameter(typeof(TData), "input");

            // Define an expression to get the value from the property
            var propertyAccess = Expression.Property(parameter, p.GetGetMethod());

            // Make sure the result of the get method is cast as an object
            var castAsObject = Expression.TypeAs(propertyAccess, typeof(object));

            // Create a lambda expression for the property access and compile it
            var lamda = Expression.Lambda<Func<TData, object>>(castAsObject, parameter);
            return lamda.Compile();
        }

        #region IDataReader Members

        public void Close()
        {
            Dispose();
        }

        public int Depth
        {
            get { return 1; }
        }

        public DataTable GetSchemaTable()
        {
            return null;
        }

        public bool IsClosed
        {
            get { return dataEnumerator == null; }
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            if (dataEnumerator == null)
            {
                throw new ObjectDisposedException("ObjectDataReader");
            }

            return dataEnumerator.MoveNext();
        }

        public int RecordsAffected
        {
            get { return -1; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dataEnumerator != null)
                {
                    dataEnumerator.Dispose();
                    dataEnumerator = null;
                }
            }
        }

        #endregion

        #region IDataRecord Members

        public int FieldCount
        {
            get { return accessors.Length; }
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            int ordinal;
            if (!ordinalLookup.TryGetValue(name, out ordinal))
            {
                throw new InvalidOperationException("Unknown parameter name " + name);
            }

            return ordinal;
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            if (dataEnumerator == null)
            {
                throw new ObjectDisposedException("ObjectDataReader");
            }

            return accessors[i](dataEnumerator.Current);
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
