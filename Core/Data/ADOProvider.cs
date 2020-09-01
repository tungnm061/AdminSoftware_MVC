using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Core.Caching;
using Core.Helper.Attributes;
using Core.Helper.Cache;
using Dapper;
using Core.Helper.Extensions;
using Core.Security.Crypt;
using StackExchange.Profiling;

namespace Core.Data
{
    /// <summary>
    /// help to gets connection string in .config file
    /// </summary>
    public class ADOProvider : IDisposable
    {
        private bool _disposed = false;
        //public DataProviderBaseClass Provider;
        private IDbTransaction _transaction;
        private string _schema;
        private CacheHelper _cacheHelper;

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IDbConnection Connection
        {
            get
            {
                if (ConnectionString != null)
                    return new SqlConnection(ConnectionString);

                return new SqlConnection(DataHelper.GetConnectionString());
            }
        }

        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public CacheHelper CacheHelper
        {
            get { return new CacheHelper(_schema); }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        //public ICacheProvider Cache
        public MemcachedProvider Cache
        {
            get { return new MemcachedProvider(_schema); }
        }

        /// <summary>
        /// Default dbo
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public ADOProvider()
        {
        }

        /// <summary>
        /// Tenants the internal.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public ADOProvider(string schema)
        {
            _schema = schema;
        }

        /// <summary>
        /// Finds the by ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Could not find the key attributes of the entity or table name Entity;id</exception>
        public virtual T FindById<T>(int id)
        {
            //http://www.contentedcoder.com/2012/12/creating-data-repository-using-dapper.html
            T item = default(T);
            var propertyEntity = ObjectExtensions.GetPrimaryKeyFromType(typeof(T));
            var propertyTableEntity = GetTableFromType(typeof(T));
            if (propertyEntity != null && propertyTableEntity != null)
            {
                var nameFieldPrimaryKey = propertyEntity.Name;
                var table = propertyTableEntity.Table;
                using (IDbConnection cn = Connection)
                {
                    cn.Open();
                    item =
                        cn.Query<T>(
                            string.Format("SELECT * FROM [{0}].{1} WHERE {2} = @ID", _schema, table,
                                          nameFieldPrimaryKey), new { ID = id }).SingleOrDefault();
                    cn.Close();
                }
                return item;
            }
            throw new ArgumentException("Could not find the key attributes of the entity or table name Entity", "id");
        }

        /// <summary>
        /// Queries the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> Query<T>(string sql, object p = null)
        {
            IEnumerable<T> item;
            using (var cn = Connection)
            {
                cn.Open();
                item = p != null ? cn.Query<T>(sql, p) : cn.Query<T>(sql);
                cn.Close();
            }
            return item;
        }

        /// <summary>
        /// Queries the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> Query(string sql, object p = null)
        {
            IEnumerable<dynamic> item;
            using (var cn = Connection)
            {
                cn.Open();
                item = p != null ? cn.Query(sql, p) : cn.Query(sql);
                cn.Close();
            }
            return item;
        }

        public virtual int Execute(string sql, object p = null)
        {
            int item;
            using (var cn = Connection)
            {
                cn.Open();
                item = p != null ? cn.Execute(sql, p) : cn.Execute(sql);
                cn.Close();
            }
            return item;
        }

        /// <summary>
        /// Procedures the specified procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">The procedure.</param>
        /// <param name="oParams">The o params.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public IEnumerable<T> Procedure<T>(string procedure, object oParams = null, bool useCache = true)
        {
            if (oParams == null) oParams = new { };

            using (MiniProfiler.Current.Step(procedure))
            {
                using (Connection)
                {
                    //var key = procedure + CacheHelper.GenerateSuffixKeyByParams(oParams) + DateTime.Now;
                    var listParams = new DynamicParameters();
                    Dictionary<string, DynamicParameters.ParamInfo> parameters = null;

                    var key = procedure + CacheHelper.GenerateSuffixKeyByParams(oParams);
                    var hashKey = Md5Util.Md5EnCrypt(key);
                    IEnumerable<T> items = null;

                    if (useCache && CacheHelper.UseCache)
                        items = Cache.Get<IEnumerable<T>>(hashKey);

                    if (useCache && CacheHelper.UseCache)
                    {
                        #region Set data ouput
                        var obj = oParams.GetType();
                        if (obj.FullName == "Dapper.DynamicParameters")
                        {
                            listParams = oParams as DynamicParameters;
                            if (listParams != null)
                            {
                                listParams.Cache = Cache.GetInstance();
                                listParams.CommandCurrent = procedure;
                                listParams.KeyOutParams = key;
                                parameters = listParams.parameters;
                            }
                        }
                        #endregion
                    }

                    //items = null;

                    if (items == null || items.Count() == 0)
                    {
                        items = Connection.Query<T>(procedure, oParams, commandType: CommandType.StoredProcedure);

                        if (useCache && CacheHelper.UseCache)
                        {
                            #region Set data ouput

                            if (parameters != null && parameters.Count > 0)
                            {
                                foreach (var parameter in parameters)
                                {
                                    var oParamInfo = parameter.Value;
                                    if (oParamInfo.ParameterDirection != ParameterDirection.Output) continue;
                                    var typeParamOutput = DataHelper.ConvertType(oParamInfo.DbType);
                                    var nameOutput = parameter.Value.Name;
                                    var keyOuput = CacheHelper.GenerateKeyCacheOutput(key, nameOutput);
                                    listParams.KeyOutParams = key;

                                    var hashKeyOuput = Md5Util.Md5EnCrypt(keyOuput);
                                    object valueOutput;
                                    switch (typeParamOutput.Name)
                                    {
                                        case "Int32":
                                            valueOutput = listParams.GetDataOutput<int>(nameOutput);
                                            Cache.Set(hashKeyOuput, valueOutput);
                                            break;
                                        case "Int16":
                                            valueOutput = listParams.GetDataOutput<Int16>(nameOutput);
                                            Cache.Set(hashKeyOuput, valueOutput);
                                            break;
                                        case "String":
                                            valueOutput = listParams.GetDataOutput<string>(nameOutput);
                                            Cache.Set(hashKeyOuput, valueOutput);
                                            //Cache.Set(hashKeyOuput, listParams.Get<string>(nameOutput));
                                            break;
                                    }
                                }
                            }
                            #endregion
                            Cache.Set(hashKey, items);
                        }
                    }
                    return items;
                }
            }
        }

        /// <summary>
        /// Procedures the query multi.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="oParams">The o params.</param>
        /// <returns></returns>
        public SqlMapper.GridReader ProcedureQueryMulti(string procedure, object oParams = null, bool useCache = true)
        {
            if (oParams == null) oParams = new { schema = _schema };
            using (MiniProfiler.Current.Step(procedure))
            {
                //var key = procedure + CacheHelper.GenerateSuffixKeyByParams(oParams);
                using (Connection)
                {
                    //SqlMapper.GridReader items = Cache.Get<SqlMapper.GridReader>(key);
                    var items = Connection.QueryMultiple(procedure, oParams, commandType: CommandType.StoredProcedure);
                    //Cache.Set(key, items);
                    return items;
                }
            }
        }

        /// <summary>
        /// Procedures the execute.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="oParams">The o params.</param>
        /// <returns></returns>
        public bool ProcedureExecute(string procedure, object oParams = null)
        {
            try
            {
                using (MiniProfiler.Current.Step(procedure))
                {
                    using (Connection)
                    {
                        Connection.Execute(procedure, oParams, commandType: CommandType.StoredProcedure);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ProcedureExecute(string procedure, int commandTimeout, object oParams = null)
        {
            try
            {
                using (MiniProfiler.Current.Step(procedure))
                {
                    using (Connection)
                    {
                        Connection.Execute(procedure, oParams, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Inserts the data using SQL bulk copy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="schema">The schema.</param>
        public void InsertDataUsingSqlBulkCopy<T>(IEnumerable<T> data, string schema = "")
        {
            try
            {
                var propertyTableEntity = GetTableFromType(typeof(T));
                if (propertyTableEntity != null)
                {
                    var schemaAction = _schema;
                    if (!string.IsNullOrEmpty(schema))
                    {
                        schemaAction = schema;
                    }
                    //var table = string.Format("{0}.{1}", _schema, propertyTableEntity.Table);
                    var table = string.Format("{0}.{1}", schemaAction, propertyTableEntity.Table);
                    using (IDbConnection cn = Connection)
                    {
                        var conn = Connection as SqlConnection;
                        if (conn == null) return;
                        if (conn.State == ConnectionState.Closed)
                            conn.Open();

                        using (var bulkCopy = new SqlBulkCopy(conn))
                        {
                            //http://www.sqlteam.com/article/use-sqlbulkcopy-to-quickly-load-data-from-your-client-to-sql-server
                            //DestinationTableName = "Person"
                            bulkCopy.NotifyAfter = 10000;
                            bulkCopy.DestinationTableName = table;
                            bulkCopy.BulkCopyTimeout = 0; // timeout unlimited
                            bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(SqlRowsCopied);
                            var fields = GetAllPropertyField<T>();
                            if (fields.Count > 0)
                            {
                                //var propertyEntity = ObjectExtensions.GetPrimaryKeyFromType(typeof(T));
                                //if (propertyEntity != null)
                                //    fields.Remove(propertyEntity.Name);

                                foreach (KeyValuePair<string, int> pair in fields)
                                {
                                    bulkCopy.ColumnMappings.Add(pair.Key, pair.Key);
                                    //bulkCopy.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
                                }
                            }
                            using (var dataReader = new ObjectDataReader<T>(data))
                            {
                                bulkCopy.WriteToServer(dataReader);
                                bulkCopy.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        /// <exception cref="System.SystemException">
        /// </exception>
        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                return;
            }

            try
            {
                _transaction.Rollback();
            }
            catch (SqlException se)
            {
                throw new SystemException(se.Message);
            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                Connection.Close();
                _transaction = null;
            }
        }


        #region Function
        /// <summary>
        /// Runs the procedure schema.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public string RunProcedureSchema(string procedure, string schema = null)
        {
            if (string.IsNullOrEmpty(schema))
            {
                if (string.IsNullOrEmpty(_schema))
                    schema = "dbo";
                else
                    schema = _schema;
            }
            var nameProcedure = string.Format("[{0}].{1}", schema, procedure);

            return nameProcedure;
        }

        /// <summary>
        /// Gets the type of the table from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The attribute ADOTable was not found;type</exception>
        public ADOTableAttribute GetTableFromType(Type type)
        {
            // Get instance of the attribute.
            var attributeAdo = (ADOTableAttribute)Attribute.GetCustomAttribute(type, typeof(ADOTableAttribute));

            if (attributeAdo == null)
            {
                throw new ArgumentException("The attribute ADOTable was not found", "type");
            }
            return attributeAdo;
        }

        /// <summary>
        /// SQLs the rows copied.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SqlRowsCopiedEventArgs"/> instance containing the event data.</param>
        private void SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            Console.WriteLine("-- Copied {0} rows.", e.RowsCopied);
        }

        /// <summary>
        /// Gets all property field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Dictionary<string, int> GetAllPropertyField<T>()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                //.Where(p => !(p.DeclaringType is Base))
                .Where(p => p.CanRead && !p.GetMethod.IsVirtual)
                .Where(p =>
                    {
                        var attributes = p.GetCustomAttributes(false);
                        return !attributes.Any(a => a is KeyAttribute)
                               && !attributes.Any(a => a is NotMappedAttribute);
                    })
                .Select((p, i) => new
                {
                    Index = i,
                    Property = p
                }).ToDictionary(
                p => p.Property.Name,
                p => p.Index,
                StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        /// <summary>
        /// Calls the protected Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ADOProvider"/> class.
        /// </summary>
        ~ADOProvider()
        {
            Dispose(false);
        }

    }
}
