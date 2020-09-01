using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Core.Data.DataAccessProvider
{
    /// <summary>
    /// Defines the DataAccessProvider implemented data provider types.
    /// </summary>
    public enum EnumDataProviderType
    {
        Access,
        Odbc,
        OleDb,
        Oracle,
        SqlClient
    }

    public abstract class DataProviderBaseClass
    {
        #region private datas, methods, constructors

        /// <summary>
        /// Private members: strConnectionString, IDbConnection, IDbCommand, IDbTransaction
        /// </summary>
        private string strConnectionString;
        public IDbConnection connection;
        private IDbCommand command;
        private IDbTransaction transaction;

        /// <summary>
        /// gets or sets connection string used to open database
        /// </summary>
        public string ConnectionString
        {
            get
            {
                // make sure connectionString is not empty or null
                if (strConnectionString.Equals(string.Empty) || strConnectionString.Length == 0)
                    throw new ArgumentException("Invalid database connection string.");
                return strConnectionString;
            }
            set
            {
                strConnectionString = value;
            }
        }

        /// <summary>
        /// Since this is an abstract class, for better documentation and readability of source code, 
        /// class is defined with an explicit protected constructor
        /// </summary>
        protected DataProviderBaseClass()
        {
        }

        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters
        /// to the provided command.
        /// </summary>
        /// <param name="commandType">typeof(CommandType) { Text, StoredProcedure, TableDirect } </param>
        /// <param name="commandText">typeof(string) { TSQL, SP }</param>
        /// <param name="parameters">typeof(IDataParameters[])</param>
        private void PrepareCommand(string commandText, CommandType commandType, IDataParameter[] parameters)
        {
            // if connection is null, provide the specific data provider connection object.
            if (connection == null)
            {
                connection = GetDataProviderConnection();
                connection.ConnectionString = ConnectionString;
            }

            // if the provided connection is not open, then open it.
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            // if the command object is null, provide the specific data provider command object.
            if (command == null)
            {
                command = GeDataProviderCommand();
            }
            
            // set all of command object's properties
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;

            // if transaction is provided, then assign it.
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            // attach the command parameters if they're provided.
            if (parameters != null)
            {
                foreach (IDataParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }
        }
        #endregion

        #region Abstract Methods

        /// <summary>
        /// Data provider specific implementation for accessing relational databases.
        /// </summary>
        internal abstract IDbConnection GetDataProviderConnection();
        /// <summary>
        /// Data provider specific implementation for executing SQL statement while connected to a data source.
        /// </summary>
        internal abstract IDbCommand GeDataProviderCommand();
        /// <summary>
        /// Data provider specific implementation for filling the DataSet.
        /// </summary>
        internal abstract IDbDataAdapter GetDataProviderDataAdapter();

        #endregion

        // generic methods implementation

        #region Database Transaction

        /// <summary>
        /// Begins a database transaction
        /// </summary>
        public void BeginTransaction()
        {
            if (transaction != null)
            {
                return;
            }

            try
            {
                connection = GetDataProviderConnection();
                connection.ConnectionString = ConnectionString;
                connection.Open();
                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch (SqlException se)
            {
                throw new SystemException(se.Message);
            }
            catch(Exception e)
            {
                throw new SystemException(e.Message);
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (transaction == null)
            {
                return;
            }

            try
            {
                transaction.Commit();
            }
            catch (SqlException se)
            {
                RollbackTransaction();
                throw new SystemException(se.Message);
            }
            catch (Exception e)
            {
                RollbackTransaction();
                throw new SystemException(e.Message);
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void RollbackTransaction()
        {
            if (transaction == null)
            {
                return;
            }

            try
            {
                transaction.Rollback();
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
                connection.Close();
                transaction = null;
            }
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                connection.Close();
            }
            catch { }
        }
        #endregion

        #region ExecuteReader

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return ExecuteReader(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a parameterized CommandText against the Connection and builds an IDataReader.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, IDbDataParameter[] parameters)
        {
            return ExecuteReader(commandText, CommandType.Text, parameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection and builds an IDataReader.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, CommandType commandType, IDbDataParameter[] parameters)
        {
            try
            {
                PrepareCommand(commandText, commandType, parameters);
                IDataReader dr = null;

                if (transaction == null)
                {
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    dr = command.ExecuteReader();
                }

                return dr;
            }
            catch (SqlException se)
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
                else
                {
                    RollbackTransaction();
                }
                throw new SystemException(se.Message);
            }
            catch (Exception e)
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
                else
                {
                    RollbackTransaction();
                }
                throw new SystemException(e.Message);
            }
        }
        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return ExecuteDataSet(commandText, commandType, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, IDataParameter[] parameters)
        {
            return ExecuteDataSet(commandText, CommandType.Text, parameters);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, IDataParameter[] paramters)
        {
            try
            {
                PrepareCommand(commandText, commandType, paramters);
                IDbDataAdapter da = GetDataProviderDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
            catch (SqlException se)
            {
                if (transaction == null)
                {
                    connection.Close();
                }
                else
                {
                    RollbackTransaction();
                }
                throw new SystemException(se.Message);
            }
            catch (Exception e)
            {
                if (transaction == null)
                {
                    connection.Close();
                }
                else
                {
                    RollbackTransaction();
                }
                throw new SystemException(e.Message);
            }
            return null;
        }
        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, commandType, null);
        }

        /// <summary>
        /// Executes an SQL parameterized statement against the Connection object of a .NET Framework data provider, 
        /// and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, IDataParameter[] parameters)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, parameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType, IDataParameter[] parameters)
        {
            try
            {
                PrepareCommand(commandText, commandType, parameters);

                int intAffectedRows = command.ExecuteNonQuery();

                return intAffectedRows;
            }
            catch (SqlException se)
            {
                if (transaction != null)
                {
                    RollbackTransaction();
                }
                throw new SystemException(se.Message); ;
            }
            catch(Exception e)
            {
                if (transaction != null)
                {
                    RollbackTransaction();
                }
                throw new SystemException(e.Message); ;
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
            }
        }
        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>System.Object.</returns>
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>System.Object.</returns>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, commandType, null);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        public object ExecuteScalar(string commandText, IDataParameter[] parameters)
        {
            return ExecuteScalar(commandText, CommandType.Text, parameters);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.SystemException">
        /// </exception>
        public object ExecuteScalar(string commandText, CommandType commandType, IDataParameter[] parameters)
        {
            try
            {
                PrepareCommand(commandText, commandType, parameters);

                object objValue = command.ExecuteScalar();

                if (objValue != null)
                {
                    return objValue;
                }
                return null;
            }
            catch (SqlException se)
            {
                if (transaction != null)
                {
                    RollbackTransaction();
                }
                throw new SystemException(se.Message);
            }
            catch(Exception e)
            {
                if (transaction != null)
                {
                    RollbackTransaction();
                }
                throw new SystemException(e.Message);
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
            }
        }
        #endregion
    }

    #region DataProvider class
    /// <summary>
	/// Loads different data access layer provider depending on the configuration settings file or the caller defined data provider type.
	/// </summary>
    public sealed class DataProvider
    {        
        /// <summary>
        /// Since this class provides only static methods, make the default constructor private to prevent  
        /// instances from being created with "new DataAccessLayerFactory()"
        /// </summary>
        private DataProvider()
        {
        }

        /// <summary>
        /// Constructs a data access layer data provider based on application configuration settings.
        /// Application configuration file must contain two keys: 
        ///		1. "DataProviderType" key, with one of the DataProviderType enumerator.
        ///		2. "ConnectionString" key, holds the database connection string.
        /// </summary>
        public static DataProviderBaseClass GetDataProvider()
        {
            // Make sure application configuration file contains required configuration keys
            /*if (System.Configuration.ConfigurationSettings.AppSettings["DataProviderType"] == null
                    || System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"] == null)
                throw new ArgumentNullException("Please specify a 'DataProviderType' and 'ConnectionString' configuration keys in the application configuration file.");*/
            if (ConfigurationManager.AppSettings["DataProviderType"] == null
                    || ConfigurationManager.AppSettings["ConnectionString"] == null)
                throw new ArgumentException("Please specify a 'DataProviderType' and 'ConnectionString' configuration keys in the application configuration file.");

            EnumDataProviderType dataProvider;

            try
            {
                // try to parse the data provider type from configuration file
                dataProvider =
                    (EnumDataProviderType)System.Enum.Parse(typeof(EnumDataProviderType),
                    System.Configuration.ConfigurationSettings.AppSettings["DataProviderType"].ToString(),
                    true);
            }
            catch
            {
                throw new ArgumentException("Invalid data access layer provider type.");
            }

            // return data access layer provider
            /*return GetDataAccessLayer(
                dataProvider,
                System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());*/
            return GetDataProvider(
                dataProvider,
                ConfigurationManager.AppSettings["ConnectionString"]);
            /*return GetDataAccessLayer(
                dataProvider,
                ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);*/
        }

        /// <summary>
        /// Constructs a data access layer based on caller specific data provider.
        /// Caller of this method must provide the database connection string through ConnectionString property.
        /// </summary>
        public static DataProviderBaseClass GetDataProvider(EnumDataProviderType dataProviderType)
        {
            return GetDataProvider(dataProviderType, null);
        }

        /// <summary>
        /// Constructs a data access layer data provider.
        /// </summary>
        public static DataProviderBaseClass GetDataProvider(EnumDataProviderType dataProviderType, string connectionString)
        {
            // construct specific data access provider class
            switch (dataProviderType)
            {
                case EnumDataProviderType.Access:
                case EnumDataProviderType.OleDb:
                    return new OleDbDataAccessLayer(connectionString);

                case EnumDataProviderType.Odbc:
                    return new OdbcDataAccessLayer(connectionString);

                case EnumDataProviderType.Oracle:
                    return new OracleDataAccessLayer(connectionString);

                case EnumDataProviderType.SqlClient:
                    return new SqlDataAccessLayer(connectionString);

                default:
                    throw new ArgumentException("Invalid data access layer provider type.");
            }
        }
    }
    #endregion
}
